using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository : IRepository<User, long>
    {
        Task<User?> GetByEmailOrUsername(string identifier);

        Task<IEnumerable<User>> GetAllAsync();
        
        Task<bool> CheckEmailExistsAsync(string email);

        Task<bool> CheckUsernameExistsAsync(string username);
    }
}