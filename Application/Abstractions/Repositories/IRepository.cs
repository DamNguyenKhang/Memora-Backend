using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IRepository<T, TKey> where T : class, IEntity<TKey>
    {
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(IEnumerable<T> items);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllByIdsAsync(IEnumerable<TKey> ids, params Expression<Func<T, object>>[] includes);
        //Task<IEnumerable<T>> GetAllAsync(
        //    Expression<Func<T, bool>> predicate,
        //    params Expression<Func<T, object>>[] includes
        //);
        //Task<T?> GetAsync(
        //    Expression<Func<T, bool>> predicate,
        //    params Expression<Func<T, object>>[] includes
        //);
    }
}
