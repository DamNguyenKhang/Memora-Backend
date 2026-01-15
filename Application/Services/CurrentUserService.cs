using System.Security.Claims;
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated == true;

        public long? UserId
        {
            get
            {
                var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return long.TryParse(id, out var userId) ? userId : null;
            }
        }

        public string? Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? UserName =>
            User?.FindFirst(ClaimTypes.Name)?.Value;
    }

}