using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enums;

namespace Application.DTOs.Request.Deck
{
    public class CreateDeckRequest
    {
        [Required]
        [MaxLength(50)]
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; } = false;

        [JsonPropertyName("difficulty")]
        public FlashcardDifficulty Difficulty { get; set; } = FlashcardDifficulty.Easy;

        [Required]
        [MinLength(1, ErrorMessage = "Deck must have at least one flashcard")]
        [JsonPropertyName("flashcards")]
        public List<CreateFlashcardRequest> Flashcards { get; set; } = new();
    }


    public class CreateFlashcardRequest
    {
        [Required]
        [JsonPropertyName("front")]
        public FlashcardSideRequest Front { get; set; } = null!;

        [Required]
        [JsonPropertyName("back")]
        public FlashcardSideRequest Back { get; set; } = null!;
    }


    public class FlashcardSideRequest
    {
        [Required]
        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;

        [JsonPropertyName("imageIndex")]
        public int? ImageIndex { get; set; }

        [JsonPropertyName("audioUrl")]
        public string? AudioUrl { get; set; }
    }
}