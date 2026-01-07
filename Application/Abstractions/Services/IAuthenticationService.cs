using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Request;
using Application.DTOs.Request.Auth;
using Application.DTOs.Response;

namespace Application.Abstractions.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponse> RegisterAsync(SignUpUserRequest request);
        Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task LogoutAsync(string refreshToken);
        Task<bool> CheckEmailExistsAsync(CheckEmailRequest email);
        Task<bool> CheckUsernameExistsAsync(CheckUsernameRequest username);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        Task VerifyEmailAsync(VerifyEmailRequest request);
        Task ResendEmailVerification(ResendEmailRequest request);
    }
}