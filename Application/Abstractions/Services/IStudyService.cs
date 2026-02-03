using Application.DTOs.Request.UserProgress;
using Application.DTOs.Response;
using Application.DTOs.Response.UserProgress;

namespace Application.Abstractions.Services
{
    public interface IStudyService
    {
        Task<GetListDeckProcessResponse> GetDecksWithProgressAsync(long userId, GetListDeckProcessRequest request);
        Task<DeckLearningDetailResponse> GetDeckForLearningAsync(long deckId);
        Task MarkFlashcardLearnedAsync(long flashcardId, MarkCardLearnedRequest request);
    }
}
