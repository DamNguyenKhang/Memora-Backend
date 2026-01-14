namespace Application.DTOs.Response.Deck
{
    public class GetListDeckResponse : PageResponse
    {
        public IEnumerable<DeckResponse>? Decks { get; set; }
    }
}