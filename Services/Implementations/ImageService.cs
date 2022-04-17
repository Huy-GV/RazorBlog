namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System;
    using Microsoft.Extensions.Logging;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using RazorBlog.Data.Constants;

    public class ImageService : IImageService
    {
        private readonly string imageDirectoryName = "images";
        private readonly string defaultProfilePictureName = "default.jpg";
        private readonly ILogger<ImageService> _logger;
        private readonly IWebHostEnvironment _webHostEnv;
        public ImageService(
            ILogger<ImageService> logger,
            IWebHostEnvironment webHostEnv)
        {
            _logger = logger;
        }
        private string ImageDirPath => Path.Combine(_webHostEnv.WebRootPath, imageDirectoryName);
        public void DeleteImage(string fileName, ImageType type, string userName)
        {
            if (fileName == defaultProfilePictureName)
            {
                _logger.LogError($"Attempt to remove default profile picture failed.");
                return;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                _logger.LogError($"Filename is empty.");
                return;
            }

            string directoryPath = Path.Combine(ImageDirPath, userName, nameof(type));
            string filePath = Path.Combine(directoryPath, fileName);
            try
            {
                File.Delete(filePath);
                _logger.LogDebug($"Deleted image of type {nameof(type)} and path {filePath}.");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogError($"Failed to remove image of type {nameof(type)} with file path: ${filePath}");
            }
            
        }

        public async Task<string> UploadBlogCoverImageAsync(IFormFile imageFile, string userName, int blogId)
        {
            var type = nameof(ImageType.BlogCover);
            var directoryPath = Path.Combine(ImageDirPath, userName, type);
            return await UploadImageAsync(directoryPath, imageFile, type);
        }

        public async Task<string> UploadProfileImageAsync(IFormFile imageFile, string userName)
        {
            var type = nameof(ImageType.ProfilePicture);
            var directoryPath = Path.Combine(ImageDirPath, userName, type);
            return await UploadImageAsync(directoryPath, imageFile, type);
        }

        private static string BuildFileName(string originalName, string type)
        {
            return string.Join
            (
                "_",
                new string[]
                {
                    DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                    type,
                    originalName.Trim('.','_','@', ' ', '#', '/', '\\', '!', '^', '&', '*')
                }
            );
        }
        private async Task<string> UploadImageAsync(
            string directoryPath,
            IFormFile imageFile,
            string type)
        {
            var formattedName = BuildFileName(imageFile.Name, type);
            var filePath = Path.Combine(directoryPath, formattedName);
            _logger.LogDebug($"File path of uploaded image is {filePath}.");
            using var stream = File.Create(filePath);
            await imageFile.CopyToAsync(stream);
            return formattedName;
        }
    }
}