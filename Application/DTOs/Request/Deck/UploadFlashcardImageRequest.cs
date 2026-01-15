using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Request.Deck
{
    public class UploadFlashcardImageRequest
    {
        public List<IFormFile> Files { get; set; }
    }
}