using System.Linq.Expressions;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public abstract class Repository<T, TKey>(ApplicationDbContext context) : IRepository<T, TKey> where T : class, IEntity<TKey>
    {
        public async Task<T> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> items)
        {
            await context.Set<T>().AddRangeAsync(items);
            return await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllByIdsAsync(IEnumerable<TKey> ids, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(e => ids.Contains(EF.Property<TKey>(e, "Id")));
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entity = await query.SingleOrDefaultAsync(e => e.Id.Equals(id));

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
