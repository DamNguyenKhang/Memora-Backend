using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Auth
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}