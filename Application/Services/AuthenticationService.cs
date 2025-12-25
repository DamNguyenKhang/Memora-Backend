using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class AuthenticationService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper) : IAuthenticationService
    {
        private readonly string jwtKey = configuration["JWT_SECRET"]!;
        private readonly double accessTokenExpirationMinutes = configuration.GetValue<double>("AppSettings:AccessTokenExpirationMinutes");
        private readonly double refreshTokenExpirationDays = configuration.GetValue<double>("AppSettings:RefreshTokenExpirationDays");

        public async Task<UserResponse> SignUpAsync(SignUpUserRequest request)
        {
            var user = mapper.Map<User>(request);
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, user.PasswordHash);
            await userRepository.AddAsync(user);
            return mapper.Map<UserResponse>(user);
        }

        public async Task<AuthenticationResponse> SignInAsync(AuthenticationRequest request)
        {
            var user = await userRepository.GetByEmailOrUsername(request.Identifier) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            bool isAuthenticated = !(user is null) && new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Success;
            if (!isAuthenticated)
            {
                throw new ApplicationException(ErrorCode.INVALID_CREDENTIALS);
            }

            return new AuthenticationResponse
            {
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                throw new ApplicationException(ErrorCode.INVALID_REFRESH_TOKEN);
            }
            return new AuthenticationResponse
            {
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }



        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
            await userRepository.UpdateAsync(user);
            return refreshToken;
        }


        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
