using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Request;
using Application.DTOs.Request.Auth;
using Application.DTOs.Response;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class AuthenticationService(
        IEmailService emailService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IEmailVerificationRepository emailVerificationRepository,
        IConfiguration configuration,
        IMapper mapper,
        ILogger<AuthenticationService> logger
    ) : IAuthenticationService
    {
        private readonly string jwtKey = configuration["Jwt:SecretKey"]!;
        private readonly double accessTokenExpirationMinutes = configuration.GetValue<double>("Jwt:AccessTokenExpirationMinutes");
        private readonly double refreshTokenExpirationDays = configuration.GetValue<double>("Jwt:RefreshTokenExpirationDays");

        private readonly string clientId = configuration["Authentication:Google:ClientId"]!;

        public async Task<UserResponse> RegisterAsync(SignUpUserRequest request)
        {
            var user = mapper.Map<User>(request);
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, user.PasswordHash);
            await userRepository.AddAsync(user);
            await SendEmailVerification(user);
            return mapper.Map<UserResponse>(user);
        }

        public async Task<AuthenticationResponse> LoginAsync(AuthenticationRequest request)
        {
            var user = await userRepository.GetByEmailOrUsername(request.Identifier) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            bool isAuthenticated = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Success;
            if (!isAuthenticated)
            {
                throw new ApplicationException(ErrorCode.INVALID_CREDENTIALS);
            }

            if (!user.IsEmailVerified)
            {
                await SendEmailVerification(user);
                throw new ApplicationException(ErrorCode.EMAIL_NOT_VERIFY);
            }

            return new AuthenticationResponse
            {
                User = mapper.Map<UserResponse>(user),
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        public async Task<AuthenticationResponse> LoginGoogleAsync(GoogleLoginRequest request)
        {
            var payload = await VerifyGoogleToken(request.IdToken) ?? throw new ApplicationException(ErrorCode.INVALID_GOOGLE_TOKEN);
            var user = await userRepository.GetByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Username = payload.Email.Split('@')[0],
                    Email = payload.Email,
                    AvatarUrl = payload.Picture,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    AuthProvider = AuthProvider.Google
                };
                await userRepository.AddAsync(user);
            }
            if (user.AuthProvider == AuthProvider.Local)
            {
                throw new ApplicationException(ErrorCode.INVALID_CREDENTIALS);
            }
            return new AuthenticationResponse
            {
                User = mapper.Map<UserResponse>(user),
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    clientId
                }
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            if (user is null || !await ValidateRefreshTokenAsync(request.RefreshToken))
            {
                throw new ApplicationException(ErrorCode.INVALID_REFRESH_TOKEN);
            }
            return new AuthenticationResponse
            {
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var refreshTokenEntity = await refreshTokenRepository.GetByTokenAsync(refreshToken) ?? throw new ApplicationException(ErrorCode.INVALID_REFRESH_TOKEN);
            refreshTokenEntity.IsRevoked = true;
            refreshTokenEntity.RevokedAt = DateTime.UtcNow;
            await refreshTokenRepository.UpdateAsync(refreshTokenEntity);
        }

        public async Task<bool> CheckEmailExistsAsync(CheckEmailRequest request)
        {
            return await userRepository.CheckEmailExistsAsync(request.Email);
        }

        public async Task<bool> CheckUsernameExistsAsync(CheckUsernameRequest request)
        {
            return await userRepository.CheckUsernameExistsAsync(request.Username);
        }

        public async Task ResendEmailVerification(ResendEmailRequest request)
        {
            var user = await userRepository.GetByEmailAsync(request.Email) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            await SendEmailVerification(user);
        }

        private async Task SendEmailVerification(User user)
        {
            var emailVerification = new EmailVerification
            {
                Token = Guid.NewGuid().ToString("N"),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                IsUsed = false,
                ExpiredAt = DateTime.UtcNow.AddMinutes(15)
            };
            await emailVerificationRepository.AddAsync(emailVerification);
            await emailService.SendVerificationEmailAsync(user.Email, emailVerification.Token);
        }

        public async Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var verification = await emailVerificationRepository.GetByTokenAsync(request.Token) ?? throw new ApplicationException(ErrorCode.EMAIL_VERIFICATION_TOKEN_NOT_FOUND);

            if (verification.IsUsed)
            {
                throw new ApplicationException(ErrorCode.USED_EMAIL_VERIFICATION_TOKEN);
            }

            if (verification.ExpiredAt < DateTime.UtcNow)
            {
                throw new ApplicationException(ErrorCode.EMAIL_VERIFICATION_TOKEN_EXPIRED);
            }

            var user = await userRepository.GetByEmailAsync(request.Email) ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                throw new ApplicationException(ErrorCode.EMAIL_NOT_MATCH);
            }

            user.IsEmailVerified = true;
            verification.IsUsed = true;
            await userRepository.UpdateAsync(user);
            await emailVerificationRepository.UpdateAsync(verification);
            return new AuthenticationResponse
            {
                User = mapper.Map<UserResponse>(user),
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        private async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = await refreshTokenRepository.GetByTokenAsync(refreshToken);
            return refreshTokenEntity is not null && refreshTokenEntity.ExpiresAt > DateTime.UtcNow && !refreshTokenEntity.IsRevoked;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.Id)
                ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new ApplicationException(ErrorCode.INVALID_CREDENTIALS);
            }

            user.PasswordHash = hasher.HashPassword(user, request.NewPassword);

            return await userRepository.UpdateAsync(user) != null;
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
            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays)
            };
            await refreshTokenRepository.AddAsync(refreshToken);
            return refreshToken.Token;
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
