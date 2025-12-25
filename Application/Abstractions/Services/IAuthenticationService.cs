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
        Task<UserResponse> SignUpAsync(SignUpUserRequest request);
        Task<AuthenticationResponse> SignInAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}