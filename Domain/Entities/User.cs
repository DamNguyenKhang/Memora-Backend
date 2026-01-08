using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        public string? PasswordHash { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public Role Role { get; set; } = Role.USER;

        [Required]
        public AuthProvider AuthProvider { get; set; } = AuthProvider.Local;

        public bool IsEmailVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        public ICollection<Deck> Decks { get; set; } = new List<Deck>();
        public ICollection<UserFlashcardProgress> FlashcardProgresses { get; set; } = new List<UserFlashcardProgress>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public ICollection<EmailVerification> EmailVerifications { get; set; } = new List<EmailVerification>();
    }
}