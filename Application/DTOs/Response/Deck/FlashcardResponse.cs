using Domain.Enums;

namespace Application.DTOs.Response.Deck
{
    public class FlashcardResponse
    {
        public long Id { get; set; }

        public FlashcardSideResponse Front { get; set; } = null!;
        public FlashcardSideResponse Back { get; set; } = null!;

        public FlashcardFlag Flag { get; set; }
        public FlashcardDifficulty Difficulty { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class FlashcardSideResponse
    {
        public string Text { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
    }
}