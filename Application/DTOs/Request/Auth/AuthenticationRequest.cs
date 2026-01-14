namespace Application.DTOs.Request.Auth
{
    public class AuthenticationRequest
    {
        public required string Identifier { get; set; }
        public required string Password { get; set; }
    }
}