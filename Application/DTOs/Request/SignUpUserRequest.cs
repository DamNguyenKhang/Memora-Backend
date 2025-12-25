using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class SignUpUserRequest
    {

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

    }
}