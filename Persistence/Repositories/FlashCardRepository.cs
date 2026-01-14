using Application.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class FlashCardRepository : Repository<Flashcard, long>, IFlashCardRepository
    {
        public FlashCardRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}