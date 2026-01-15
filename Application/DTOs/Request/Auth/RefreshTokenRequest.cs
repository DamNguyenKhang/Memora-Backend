namespace Application.DTOs.Request.Auth
{
    public class RefreshTokenRequest
    {
        public long UserId { get; set; }
        public string? RefreshToken { get; set; }
    }
}