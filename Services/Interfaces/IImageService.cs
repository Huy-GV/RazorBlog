using Microsoft.AspNetCore.Http;
using RazorBlog.Data.Constants;
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
        Task<string> UploadBlogCoverImageAsync(IFormFile imageFile, string userName, int blogId);

        /// <summary>
        /// Upload the profile image of a user.
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns>The name of the uploaded image.</returns>
        Task<string> UploadProfileImageAsync(IFormFile imageFile, string userName);

        /// <summary>
        /// Called when user wants to revert their profile image to default or upload a new profile/ blog cover image
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        void DeleteImage(string fileName, ImageType type, string userName);
    }
}