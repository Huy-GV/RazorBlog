namespace RazorBlog.Services.Implementations;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RazorBlog.Data.Constants;
using RazorBlog.Services.Communications;
using RazorBlog.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

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
        _webHostEnv = webHostEnv;
    }

    private string ImageDirPath => Path.Combine(_webHostEnv.WebRootPath, imageDirectoryName);

    public async Task<Result<Empty, Error>> DeleteImage(string fileName, ImageType type)
    {
        if (fileName == defaultProfilePictureName)
        {
            _logger.LogError($"Attempt to remove default profile picture failed.");
            return ResultUtil.Failure(ServiceCode.InvalidArgument);
        }

        string directoryPath = Path.Combine(ImageDirPath, nameof(type));
        string filePath = Path.Combine(directoryPath, fileName);
        try
        {
            File.Delete(filePath);
            _logger.LogDebug($"Deleted image of type {nameof(type)} and path {filePath}.");
            return ResultUtil.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError($"Failed to remove image of type {nameof(type)} with file path: ${filePath}");
            return ResultUtil.Failure(ServiceCode.InternalError);
        }
    }

    public async Task<string> UploadBlogCoverImageAsync(IFormFile imageFile)
    {
        var type = nameof(ImageType.BlogCover);
        var directoryPath = Path.Combine(ImageDirPath, type);
        return await UploadImageAsync(directoryPath, imageFile, type);
    }

    public async Task<string> UploadProfileImageAsync(IFormFile imageFile)
    {
        var type = nameof(ImageType.ProfilePicture);
        var directoryPath = Path.Combine(ImageDirPath, type);
        return await UploadImageAsync(directoryPath, imageFile, type);
    }

    private string BuildFileName(string originalName, string type)
    {
        return string.Join
        (
            "_",
            new string[]
            {
                DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                type,
                originalName.Trim('.','_','@', ' ', '#', '/', '\\', '!', '^', '&', '*'),
            }
        );
    }

    private async Task<string> UploadImageAsync(
        string directoryPath,
        IFormFile imageFile,
        string type)
    {
        var formattedName = BuildFileName(imageFile.FileName, type);
        var filePath = Path.Combine(directoryPath, formattedName);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using var stream = File.Create(filePath);
        await imageFile.CopyToAsync(stream);
        _logger.LogInformation($"File path of uploaded image is {formattedName}.");

        return formattedName;
    }
}