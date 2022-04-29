using RazorBlog.Data.DTOs;
using RazorBlog.Services.Communications;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RazorBlog.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<Result<UploadedFile, Error>> UploadProfilePicture();

        /// <summary>
        /// Set profile picture to the default silhouette
        /// </summary>
        /// <returns></returns>
        Task<Result<Empty, Error>> RemoveProfilePicture();

        Task<Result<Empty, Error>> DeactivateAccount();

        Task<Result<IList<DetailedBlogDto>, Error>> GetBlogPostHistory();

        /// <summary>
        /// Update personal details of a user.
        /// </summary>
        /// <returns>A result whose data is the ID of the updated user.</returns>
        Task<Result<string, Error>> UpdatePersonalDetails();
    }
}