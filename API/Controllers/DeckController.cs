using Application.Abstractions.Services;
using Application.DTOs.Request.Deck;
using Application.DTOs.Response;
using Application.DTOs.Response.Deck;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeckController(IDeckService deckService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<DeckResponse>>> CreateDeck([FromBody] CreateDeckRequest request)
        {
            var response = await deckService.CreateDeckAsync(request);
            return new ApiResponse<DeckResponse>
            {
                Result = response,
                Message = "Create deck successfully"
            };
        }

        [HttpGet("get-deck/{userId}")]
        public async Task<ActionResult<ApiResponse<GetListDeckResponse>>> GetDeckByOwnerId(long userId, [FromQuery] GetListDeckRequest request)
        {
            var response = await deckService.GetDeckByOwnerId(userId, request);
            return new ApiResponse<GetListDeckResponse>
            {
                Result = response,
                Message = "Create deck successfully"
            };
        }

    }
}