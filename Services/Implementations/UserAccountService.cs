namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System.Threading.Tasks;

    public class UserAccountService : IUserAccountService
    {
        public Task<Result> DeactivateAccount()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> GetBlogPostHistory()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> RemoveProfilePicture()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> UpdatePersonalDetails()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> UploadProfilePicture()
        {
            throw new System.NotImplementedException();
        }
    }
}