using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class Flashcard : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long DeckId { get; set; }

        // Front & Back
        [Required]
        public FlashcardSide Front { get; set; } = new();

        [Required]
        public FlashcardSide Back { get; set; } = new();

        // Flag
        public FlashcardFlag Flag { get; set; } = FlashcardFlag.None;

        public FlashcardDifficulty Difficulty { get; set; } = FlashcardDifficulty.Easy;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        // Navigation
        public Deck Deck { get; set; } = null!;
        public ICollection<UserFlashcardProgress> Progresses { get; set; } = new List<UserFlashcardProgress>();
    }

    public class FlashcardSide
    {
        [Required]
        public string Text { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
    }

}