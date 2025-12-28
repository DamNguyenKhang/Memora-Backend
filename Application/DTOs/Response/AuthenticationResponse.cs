namespace Application.DTOs.Response
{
    public class AuthenticationResponse
    {
        public UserResponse? User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}