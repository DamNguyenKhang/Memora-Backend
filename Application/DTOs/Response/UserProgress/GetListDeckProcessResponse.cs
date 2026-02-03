namespace Application.DTOs.Response.UserProgress
{
    public class GetListDeckProcessResponse : PageResponse
    {
        public IEnumerable<DeckLearningDetailResponse>? Decks { get; set; }
    }
}