using RazorBlog.Services.Communications;
using System.Threading.Tasks;

namespace RazorBlog.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<Result> UploadProfilePicture();

        /// <summary>
        /// Set profile picture to the default silhouette
        /// </summary>
        /// <returns></returns>
        Task<Result> RemoveProfilePicture();

        Task<Result> DeactivateAccount();

        Task<Result> GetBlogPostHistory();

        Task<Result> UpdatePersonalDetails();
    }
}