using Application.Abstractions.Repositories;
using Application.Specifications;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class FolderRepository : Repository<Folder, long>, IFolderRepository
    {
        public FolderRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<(List<Folder>, int)> GetPagedAsync(ISpecification<Folder> spec, int page, int size)
        {
            IQueryable<Folder> query = _dbSet;

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
