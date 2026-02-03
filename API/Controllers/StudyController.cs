using Application.Abstractions.Services;
using Application.DTOs.Request.UserProgress;
using Application.DTOs.Response;
using Application.DTOs.Response.UserProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudyController(IStudyService studyService) : ControllerBase
    {
        [HttpPost("flashcards/{id}/mark-learned")]
        public async Task<ActionResult<ApiResponse>> MarkLearned(long id, [FromBody] MarkCardLearnedRequest request)
        {
            await studyService.MarkFlashcardLearnedAsync(id, request);
            return new ApiResponse
            {
                Message = "Flashcard marked as learned"
            };
        }

        [HttpGet("decks/{id}/learn")]
        public async Task<ActionResult<ApiResponse<DeckLearningDetailResponse>>> GetDeckForLearning(long id)
        {
             var result = await studyService.GetDeckForLearningAsync(id);
             return new ApiResponse<DeckLearningDetailResponse>
             {
                 Result = result,
                 Message = "Get deck for learning successfully"
             };
        }
    }
}
