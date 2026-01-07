using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Auth
{
    public class ResendEmailRequest
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}