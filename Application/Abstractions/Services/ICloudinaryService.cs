using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;

namespace Application.Abstractions.Services
{
    public interface ICloudinaryService
    {
        Task<UploadFlashcardImageResponse> UploadAsync(UploadFlashcardImageRequest request);
    }
}