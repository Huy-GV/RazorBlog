namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Data.DTOs;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserAccountService : IUserAccountService
    {
        public Task<Result<Empty, Error>> DeactivateAccount()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<IList<DetailedBlogDto>, Error>> GetBlogPostHistory()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Empty, Error>> RemoveProfilePicture()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<string, Error>> UpdatePersonalDetails()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<UploadedFile, Error>> UploadProfilePicture()
        {
            throw new System.NotImplementedException();
        }
    }
}