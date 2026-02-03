using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserFlashcardProgressRepository : IRepository<UserFlashcardProgress, long>
    {
        Task<Dictionary<long, int>> GetLearnedCardCountByDeckIdsAsync(long userId, List<long> deckIds);

        Task<UserFlashcardProgress?> GetByUserIdAndFlashcardIdAsync(long userId, long flashcardId);

        Task<int> CountLearnedInDeckAsync(long userId, long deckId);
    }
}