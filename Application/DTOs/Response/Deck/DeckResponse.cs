using Application.DTOs.Response.Auth;
using Domain.Entities;

namespace Application.DTOs.Response.Deck
{
    public class DeckResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsPublic { get; set; }
        public List<string>? Tags { get; set; } = new();
        public int FlashcardCount { get; set; }
        public List<FlashcardResponse>? Flashcards { get; set; }

        public OwnerDeckResponse Owner { get; set; } = null!;

        public bool IsOwner { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class OwnerDeckResponse
    {
        public long Id { get; set; }
        public string username { get; set; } = null!;
        public string? avatarUrl { get; set; }
    }
}