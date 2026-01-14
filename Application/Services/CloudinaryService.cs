using Application.Abstractions.Services;
using Application.DTOs.Request.Deck;
using Application.DTOs.Response.Deck;
using Application.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) ||
                string.IsNullOrEmpty(apiKey) ||
                string.IsNullOrEmpty(apiSecret))
            {
                throw new Exception("Cloudinary configuration is missing");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<UploadFlashcardImageResponse> UploadAsync(UploadFlashcardImageRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                throw new ApplicationException(ErrorCode.EMPTY_FILE);

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "flashcards",
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ApplicationException(ErrorCode.FIlE_UPLOAD_ERROR);

            return new UploadFlashcardImageResponse
            {
                ImageUrl = result.SecureUrl.ToString()
            };
        }
    }
}
