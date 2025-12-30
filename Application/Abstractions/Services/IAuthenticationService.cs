using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Abstractions.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponse> RegisterAsync(SignUpUserRequest request);
        Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(string refreshToken);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);
    }
}