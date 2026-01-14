using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Request.Deck
{
    public class UploadFlashcardImageRequest
    {
        public IFormFile File { get; set; }
    }
}