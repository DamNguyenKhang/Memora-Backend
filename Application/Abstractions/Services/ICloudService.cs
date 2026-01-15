using Application.DTOs.Response.Deck;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services
{
    public interface ICloudService
    {
        Task<List<string>> UploadAsync(List<IFormFile> files);
    }
}