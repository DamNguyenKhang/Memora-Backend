using Application.Abstractions.Services;
using Application.DTOs.Request;
using Application.DTOs.Request.Auth;
using Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] SignUpUserRequest request)
        {
            return new ApiResponse<UserResponse>
            {
                Result = await authService.RegisterAsync(request),
                Message = "Create new user successfully"
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login(AuthenticationRequest request)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax, 
                Secure = false,              
                Path = "/"
            };
            var authResponse = await authService.LoginAsync(request);
            Response.Cookies.Append("refreshToken", authResponse.RefreshToken, cookieOptions);
            return new ApiResponse<AuthenticationResponse>
            {
                Result = authResponse,
                Message = "Login successfully"
            };
        }

        [HttpPost("login-google")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> LoginGoogle(GoogleLoginRequest request)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax, 
                Secure = false,              
                Path = "/"
            };
            var authResponse = await authService.LoginGoogleAsync(request);
            Response.Cookies.Append("refreshToken", authResponse.RefreshToken, cookieOptions);
            return new ApiResponse<AuthenticationResponse>
            {
                Result = authResponse,
                Message = "Login successfully"
            };
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> RefreshToken(RefreshTokenRequest request)
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized("Refresh token missing");
            request.RefreshToken = refreshToken;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax, 
                Secure = false,              
                Path = "/"
            };
            var authResponse = await authService.RefreshTokenAsync(request);
            Response.Cookies.Append("refreshToken", authResponse.RefreshToken, cookieOptions);

            return new ApiResponse<AuthenticationResponse>
            {
                Result = authResponse,
                Message = "Refresh token successfully"
            };
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse>> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized("Refresh token missing");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax, 
                Secure = false,              
                Path = "/"
            };
            Response.Cookies.Append("refreshToken", "", cookieOptions);
            await authService.LogoutAsync(refreshToken);
            return new ApiResponse
            {
                Message = "Logout successfully"
            };
        }

        [HttpPost("check-email-exist")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmailExists([FromBody] CheckEmailRequest request)
        {
            bool isEmailExists = await authService.CheckEmailExistsAsync(request);
            return new ApiResponse<bool>
            {
                Result = isEmailExists,
                Message = isEmailExists ? "Email exists" : "Email does not exist"
            };
        }

        [HttpPost("check-username-exist")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckUsernameExists([FromBody] CheckUsernameRequest request)
        {
            bool isUsernameExists = await authService.CheckUsernameExistsAsync(request);
            return new ApiResponse<bool>
            {
                Result = isUsernameExists,
                Message = isUsernameExists ? "Username exists" : "Username does not exist"
            };
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            bool isSuccess = await authService.ChangePasswordAsync(request);
            return new ApiResponse<bool>
            {
                Result = isSuccess,
                Message = isSuccess ? "Change password successfully" : "Change password failed"
            };
        }

        [HttpPost("resend-email-verification")]
        public async Task<ActionResult<ApiResponse>> ResendEmailVerification([FromBody] ResendEmailRequest request)
        {
            await authService.ResendEmailVerification(request);
            return new ApiResponse
            {
                Message = "Resend email verification successfully"
            };
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax, 
                Secure = false,              
                Path = "/"
            };
            var authResponse = await authService.VerifyEmailAsync(request);
            Response.Cookies.Append("refreshToken", authResponse.RefreshToken, cookieOptions);
            return new ApiResponse<AuthenticationResponse>
            {
                Result = authResponse,
                Message = "Verify email successfully"
            };
        }
    }
}