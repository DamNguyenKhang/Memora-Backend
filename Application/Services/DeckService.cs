using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;
using Application.Specifications;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class DeckService(
        IFlashCardRepository flashCardRepository,
        IDeckRepository deckRepository,
        IMapper mapper
    ) : IDeckService
    {
        public async Task<DeckResponse> CreateDeckAsync(CreateDeckRequest request)
        {
            var deck = mapper.Map<Deck>(request);
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
    }
}