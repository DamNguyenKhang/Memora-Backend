using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, long>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}