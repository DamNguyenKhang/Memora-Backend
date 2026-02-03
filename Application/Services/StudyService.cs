using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Request.UserProgress;
using Application.DTOs.Response.UserProgress;
using AutoMapper;
using Domain.Entities;
using Application.Specifications;
using Application.Exceptions;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class StudyService : IStudyService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly IUserFlashcardProgressRepository _progressRepository;
        private readonly IFlashCardRepository _flashcardRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public StudyService(
            IDeckRepository deckRepository,
            IUserFlashcardProgressRepository progressRepository,
            IFlashCardRepository flashcardRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _deckRepository = deckRepository;
            _progressRepository = progressRepository;
            _flashcardRepository = flashcardRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<GetListDeckProcessResponse> GetDecksWithProgressAsync(long userId, GetListDeckProcessRequest request)
        {
            var spec = new DeckSpecification();
            spec.WithOwner(userId);
            spec.WithFolder(request.FolderId);
            spec.NotDeleted();

            var (items, totalItems) = await _deckRepository.GetPagedAsync(spec, request.Page, request.Size);

            var deckIds = items.Select(d => d.Id).ToList();

            var totalDict = deckIds.Count == 0
                ? new Dictionary<long, int>()
                : await _flashcardRepository.GetTotalCardCountByDeckIdsAsync(deckIds);

            var learnedDict = deckIds.Count == 0
                ? new Dictionary<long, int>()
                : await _progressRepository.GetLearnedCardCountByDeckIdsAsync(userId, deckIds);

            var dtos = _mapper.Map<List<DeckLearningDetailResponse>>(items);

            foreach (var dto in dtos)
            {
                totalDict.TryGetValue(dto.Id, out var total);
                learnedDict.TryGetValue(dto.Id, out var learned);

                dto.TotalCardCount = total;
                dto.LearnedCardCount = learned;
                dto.ProgressPercentage = total == 0 ? 0 : Math.Round(learned * 100.0 / total, 2);
            }

            return new GetListDeckProcessResponse
            {
                Page = request.Page,
                Size = request.Size,
                TotalItems = totalItems,
                Decks = dtos
            };
        }

        public async Task<DeckLearningDetailResponse> GetDeckForLearningAsync(long deckId)
        {
            var userId = _currentUserService.UserId ?? throw new ApplicationException(ErrorCode.UNAUTHENTICATED);
            var deck = await _deckRepository.GetByIdAsync(deckId, d => d.Flashcards);
            if (deck == null) throw new ApplicationException(ErrorCode.DECK_NOT_FOUND);

            var learnedCount = await _progressRepository.CountLearnedInDeckAsync(userId, deckId);

            var response = _mapper.Map<DeckLearningDetailResponse>(deck);
            response.TotalCardCount = deck.Flashcards.Count;
            response.LearnedCardCount = learnedCount;
            response.ProgressPercentage = response.TotalCardCount > 0
                ? Math.Round(learnedCount * 100.0 / response.TotalCardCount, 2)
                : 0;

            return response;
        }

        public async Task MarkFlashcardLearnedAsync(long flashcardId, MarkCardLearnedRequest request)
        {
            var userId = _currentUserService.UserId ?? throw new ApplicationException(ErrorCode.UNAUTHENTICATED);

            var flashcard = await _flashcardRepository.GetByIdAsync(flashcardId);
            if (flashcard == null) throw new KeyNotFoundException("Flashcard not found");

            // Tìm progress record theo (userId, flashcardId)
            var existingProgress = await _progressRepository
                .GetByUserIdAndFlashcardIdAsync(userId, flashcardId);

            var now = DateTime.UtcNow;

            if (existingProgress != null)
            {
                existingProgress.LastReviewedAt = now;
                existingProgress.UpdatedAt = now;

                await _progressRepository.UpdateAsync(existingProgress);
            }
            else
            {
                var progress = new UserFlashcardProgress
                {
                    UserId = userId,
                    FlashcardId = flashcardId,
                    LastReviewedAt = now,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _progressRepository.AddAsync(progress);
            }
        }
    }
}
