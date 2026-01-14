using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;

namespace Application.Abstractions.Services
{
    public interface IDeckService
    {
        Task<DeckResponse> CreateDeckAsync(CreateDeckRequest request);

        Task<GetListDeckResponse> GetDeckByOwnerId(long userId, GetListDeckRequest request);
    }
}