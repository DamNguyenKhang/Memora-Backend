using Application.Abstractions.Services;
using Application.DTOs.Request.Deck;
using Application.DTOs.Response;
using Application.DTOs.Response.Deck;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/cloud")]
    public class CloudinaryController(ICloudService uploadService) : ControllerBase
    {
        // [HttpPost("upload/flashcard-image")]
        // public async Task<ActionResult<ApiResponse<UploadFlashcardImageResponse>>> UploadFlashcardImage([FromForm] UploadFlashcardImageRequest request)
        // {
        //     var response = await uploadService.UploadAsync(request);

        //     return new ApiResponse<UploadFlashcardImageResponse>
        //     {
        //         Result = response,
        //         Message = "Image uploaded successfully",
        //     };
        // }
    }
}