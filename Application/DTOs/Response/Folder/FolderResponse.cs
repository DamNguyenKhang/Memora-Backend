using Application.DTOs.Response.Deck;

namespace Application.DTOs.Response.Folder
{
    public class FolderResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public long UserId { get; set; }

        public List<DeckResponse>? decks { get; set; }

        public int DeckCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}