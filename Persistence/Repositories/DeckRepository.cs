using Application.Abstractions.Repositories;
using Application.Specifications;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DeckRepository : Repository<Deck, long>, IDeckRepository
    {
        public DeckRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(List<Deck>, int)> GetPagedAsync(
            ISpecification<Deck> spec,
            int page,
            int size)
        {
            IQueryable<Deck> query = _dbSet;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (items, totalItems);
        }
    }
}