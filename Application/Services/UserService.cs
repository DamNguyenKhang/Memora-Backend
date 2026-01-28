using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Response.Auth;
using Application.Exceptions;
using AutoMapper;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class UserService(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper) : IUserService
    {
        public async Task<IEnumerable<UserResponse>?> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse?> GetUserById(long userId)
        {
            var user = await userRepository.GetByIdAsync(userId) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            return mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetCurrentUserAsync()
        {
            var userId = currentUserService.UserId ?? throw new ApplicationException(ErrorCode.UNAUTHENTICATED);
            var user = await userRepository.GetByIdAsync(userId) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            return mapper.Map<UserResponse>(user);
        }
    }
}