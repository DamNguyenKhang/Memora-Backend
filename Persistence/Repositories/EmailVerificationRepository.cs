using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class EmailVerificationRepository : Repository<EmailVerification, long>, IEmailVerificationRepository
    {
        public EmailVerificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<EmailVerification?> GetByTokenAsync(string verifyToken)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Token == verifyToken);
        }
    }
}