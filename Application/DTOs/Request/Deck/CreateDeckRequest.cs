using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Request.Deck
{
    public class CreateDeckRequest
    {
        [Required]
        public long OwnerId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public List<string>? Tags { get; set; }

        public bool IsPublic { get; set; } = false;

        public FlashcardDifficulty Difficulty { get; set; } = FlashcardDifficulty.Easy;

        [Required]
        [MinLength(1, ErrorMessage = "Deck must have at least one flashcard")]
        public List<CreateFlashcardRequest> Flashcards { get; set; } = new();
    }

    public class CreateFlashcardRequest
    {
        [Required]
        public FlashcardSideRequest Front { get; set; } = null!;

        [Required]
        public FlashcardSideRequest Back { get; set; } = null!;
    }

    public class FlashcardSideRequest
    {
        [Required]
        public string Text { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
    }
}