using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserFlashcardProgressRepository : Repository<UserFlashcardProgress, long>, IUserFlashcardProgressRepository
    {
        public UserFlashcardProgressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Dictionary<long, int>> GetLearnedCardCountByDeckIdsAsync(long userId, List<long> deckIds)
        {
            if (deckIds == null || deckIds.Count == 0)
                return new Dictionary<long, int>();

            return await (
                from f in _context.Flashcards
                join p in _context.UserFlashcardProgresses on f.Id equals p.FlashcardId
                where deckIds.Contains(f.DeckId)
                      && p.UserId == userId
                      && p.LastReviewedAt != null
                group p by f.DeckId into g
                select new
                {
                    DeckId = g.Key,
                    Learned = g.Select(x => x.FlashcardId).Distinct().Count()
                }
            ).ToDictionaryAsync(x => x.DeckId, x => x.Learned);
        }

        public async Task<UserFlashcardProgress?> GetByUserIdAndFlashcardIdAsync(long userId, long flashcardId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.UserId == userId && x.FlashcardId == flashcardId);
        }

        public async Task<int> CountLearnedInDeckAsync(long userId, long deckId)
        {
            return await (
                from p in _context.UserFlashcardProgresses.AsNoTracking()
                join f in _context.Flashcards.AsNoTracking()
                    on p.FlashcardId equals f.Id
                where p.UserId == userId
                    && f.DeckId == deckId
                    && p.LastReviewedAt != null
                select p.Id
            ).CountAsync();
        }
    }
}