using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;

namespace Application.Abstractions.Services
{
    public interface IDeckService
    {
        Task<DeckResponse> CreateDeckAsync(CreateDeckMultipartRequest request);

        Task<GetListDeckResponse> GetDeckByOwnerId(long userId, GetListDeckRequest request);

        Task<DeckResponse?> GetDeckByIdAsync(long deckId);
    }
}