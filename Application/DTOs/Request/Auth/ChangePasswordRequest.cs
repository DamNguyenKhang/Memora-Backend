using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Auth
{
    public class ChangePasswordRequest
    {
        public long Id { get; set; }
        [Required]
        public string CurrentPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}