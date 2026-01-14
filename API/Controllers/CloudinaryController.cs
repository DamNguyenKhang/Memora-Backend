using Application.Abstractions.Services;
using Application.DTOs.Request.Deck;
using Application.DTOs.Response;
using Application.DTOs.Response.Deck;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/cloud")]
    public class CloudinaryController(ICloudinaryService uploadService) : ControllerBase
    {
        [HttpPost("uploads/flashcard")]
        public async Task<ActionResult<ApiResponse<UploadFlashcardImageResponse>>> Upload([FromForm] UploadFlashcardImageRequest request)
        {
            var response = await uploadService.UploadAsync(request);

            return new ApiResponse<UploadFlashcardImageResponse>
            {
                Result = response,
                Message = "Image uploaded successfully",
            };
        }


    }
}