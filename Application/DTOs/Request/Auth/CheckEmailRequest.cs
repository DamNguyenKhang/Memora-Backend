using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Auth
{
    public class CheckEmailRequest
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}