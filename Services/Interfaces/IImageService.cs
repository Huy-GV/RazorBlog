using Microsoft.AspNetCore.Http;
using RazorBlog.Data.Constants;
using RazorBlog.Services.Communications;
using System.Threading.Tasks;

namespace RazorBlog.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Upload the cover image of a blog.
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns>The name of the uploaded image.</returns>
        Task<string> UploadBlogCoverImageAsync(IFormFile imageFile);

        /// <summary>
        /// Upload the profile image of a user.
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns>The name of the uploaded image.</returns>
        Task<string> UploadProfileImageAsync(IFormFile imageFile);

        /// <summary>
        /// Called when user wants to revert their profile image to default or upload a new profile/ blog cover image
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        Result<Empty, Error> DeleteImage(string fileName, ImageType type);
    }
}