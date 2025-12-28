using Application.DTOs.Response;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>?> GetAllAsync();
    }
}