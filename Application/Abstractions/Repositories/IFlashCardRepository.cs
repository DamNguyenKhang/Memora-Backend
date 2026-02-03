using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IFlashCardRepository : IRepository<Flashcard, long>
    {
        Task<Dictionary<long, int>> GetTotalCardCountByDeckIdsAsync(List<long> deckIds);
    }
}