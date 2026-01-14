using Application.DTOs.Request.Auth;
using Application.DTOs.Response.Auth;

namespace Application.Abstractions.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponse> RegisterAsync(SignUpUserRequest request);
        Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> LoginGoogleAsync(GoogleLoginRequest request);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task LogoutAsync(string refreshToken);
        Task<bool> CheckEmailExistsAsync(CheckEmailRequest email);
        Task<bool> CheckUsernameExistsAsync(CheckUsernameRequest username);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request);
        Task ResendEmailVerification(ResendEmailRequest request);
    }
}