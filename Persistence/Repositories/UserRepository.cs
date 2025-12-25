using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserRepository : Repository<User, long>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailOrUsername(string identifier)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == identifier || u.Username == identifier);
        }
    }
}