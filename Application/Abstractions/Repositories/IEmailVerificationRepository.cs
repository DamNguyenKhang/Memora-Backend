using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IEmailVerificationRepository : IRepository<EmailVerification, long>
    {
        Task<EmailVerification?> GetByTokenAsync(string verifyToken);
    }
}