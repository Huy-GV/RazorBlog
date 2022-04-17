using Microsoft.AspNetCore.Http;
using System;
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

        void DeleteImage(string fileName);
    }
}