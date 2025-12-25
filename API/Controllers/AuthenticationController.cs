using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Services;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authService) : ControllerBase
    {
        [HttpPost("sign-up")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateUser([FromBody] SignUpUserRequest request)
        {
            return new ApiResponse<UserResponse>
            {
                Result = await authService.SignUpAsync(request),
                Message = "Create new user successfully"
            };
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login(AuthenticationRequest request)
        {
            return new ApiResponse<AuthenticationResponse>
            {
                Result = await authService.SignInAsync(request),
                Message = "Login successfully"
            };
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> RefreshToken(RefreshTokenRequest request)
        {
            return new ApiResponse<AuthenticationResponse>
            {
                Result = await authService.RefreshTokenAsync(request),
                Message = "Refresh token successfully"
            };
        }
    }
}