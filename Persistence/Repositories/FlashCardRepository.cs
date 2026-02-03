using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class FlashCardRepository : Repository<Flashcard, long>, IFlashCardRepository
    {
        public FlashCardRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Dictionary<long, int>> GetTotalCardCountByDeckIdsAsync(List<long> deckIds)
        {
            if (deckIds == null || deckIds.Count == 0)
                return new Dictionary<long, int>();

            return await _dbSet
                .Where(f => deckIds.Contains(f.DeckId))
                .GroupBy(f => f.DeckId)
                .Select(g => new { DeckId = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.DeckId, x => x.Total);
        }
    }
}