namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {
        public string BuildFileName(string originalName)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteImage(string fileName)
        {
            throw new System.NotImplementedException();
        }

        public Task UploadBlogImageAsync(IFormFile imageFile, string fileName)
        {
            throw new System.NotImplementedException();
        }

        public Task UploadProfileImageAsync(IFormFile imageFile, string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}