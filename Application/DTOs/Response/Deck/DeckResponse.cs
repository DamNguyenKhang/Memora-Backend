namespace Application.DTOs.Response.Deck
{
    public class DeckResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsPublic { get; set; }
        public int FlashcardCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}