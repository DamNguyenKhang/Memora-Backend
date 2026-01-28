using System.Text.Json;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;
using Application.Exceptions;
using Application.Specifications;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class DeckService(
        IFlashCardRepository flashCardRepository,
        IDeckRepository deckRepository,
        ICloudService cloudService,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<IDeckService> logger
    ) : IDeckService
    {
        public async Task<DeckResponse> CreateDeckAsync(CreateDeckMultipartRequest request)
        {
            var deckDto = JsonSerializer.Deserialize<CreateDeckRequest>(request.Deck) ?? throw new ApplicationException(ErrorCode.INTERNAL_ERROR);
            var imageUrls = new List<string>(); ;
            if (request.Images != null && request.Images.Any())
            {
                imageUrls = await cloudService.UploadAsync(request.Images);
            }
            var deck = mapper.Map<Deck>(deckDto);
            deck.OwnerId = currentUserService.UserId ?? throw new ApplicationException(ErrorCode.UNAUTHENTICATED);

            for (int i = 0; i < deckDto.Flashcards.Count; i++)
            {
                var card = deckDto.Flashcards[i];
                var flashcard = deck.Flashcards.ElementAt(i);

                if (card.Front.ImageIndex.HasValue)
                {
                    var index = card.Front.ImageIndex.Value;

                    if (index >= 0 && index < imageUrls.Count)
                    {
                        flashcard.Front.ImageUrl = imageUrls[index];
                    }
                }
            }
            await deckRepository.AddAsync(deck);
            return mapper.Map<DeckResponse>(deck);
        }

        public async Task<GetListDeckResponse> GetDeckByOwnerId(long userId, GetListDeckRequest request)
        {
            var spec = new DeckSpecification();
            spec.WithOwner(userId);
            spec.NotDeleted();

            var (items, totalItems) = await deckRepository.GetPagedAsync(spec, request.Page, request.Size);
            return new GetListDeckResponse()
            {
                Page = request.Page,
                Size = request.Size,
                TotalItems = totalItems,
                Decks = mapper.Map<IEnumerable<DeckResponse>>(items)
            };
        }

        public async Task<DeckResponse?> GetDeckByIdAsync(long deckId)
        {
            var deck = await deckRepository.GetByIdAsync(deckId, d => d.Flashcards, d => d.Owner) ?? throw new ApplicationException(ErrorCode.DECK_NOT_FOUND);
            var deckResponse = mapper.Map<DeckResponse>(deck);
            deckResponse.IsOwner = deck.OwnerId == currentUserService.UserId;
            return deckResponse;
        }
    }
}