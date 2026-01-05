using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Auth
{
    public class CheckUsernameRequest
    {
        [Required]
        public string Username { get; set; } = null!;
    }
}