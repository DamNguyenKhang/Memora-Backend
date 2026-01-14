using Application.DTOs.Response.Auth;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>?> GetAllAsync();

        Task<UserResponse?> GetUserById(long userId);
    }
}