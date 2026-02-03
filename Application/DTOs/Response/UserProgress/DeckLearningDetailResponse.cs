using Application.DTOs.Response.Deck;

namespace Application.DTOs.Response.UserProgress
{
    public class DeckLearningDetailResponse : DeckResponse
    {

        public int LearnedCardCount { get; set; }
        public double ProgressPercentage { get; set; }
    }
}
