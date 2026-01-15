using Application.Abstractions.Services;
using Application.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class CloudService : ICloudService
    {
        private readonly Cloudinary _cloudinary;

        public CloudService(IConfiguration configuration)
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

        public async Task<List<string>> UploadAsync(List<IFormFile> files)
        {
            if (files == null || !files.Any())
                throw new ApplicationException(ErrorCode.EMPTY_FILE);

            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "flashcards/images",
                    Transformation = new Transformation()
                        .Quality("auto")
                        .FetchFormat("auto")
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ApplicationException(
                        ErrorCode.FILE_UPLOAD_ERROR,
                        result.Error?.Message
                    );

                imageUrls.Add(result.SecureUrl.ToString());
            }

            return imageUrls;
        }


    }
}
