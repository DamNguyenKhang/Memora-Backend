using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository : IRepository<User, long>
    {
        Task<User?> GetByEmailOrUsername(string identifier);

        Task<IEnumerable<User>> GetAllAsync();
    }
}