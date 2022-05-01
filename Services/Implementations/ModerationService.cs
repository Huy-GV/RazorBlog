namespace RazorBlog.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using RazorBlog.Models;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System;
    using System.Threading.Tasks;

    public class ModerationService : IModerationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Task<Result<Empty, Error>> AssignModerator(int topicId, string userId, DateTime? schedule = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Empty, Error>> BanUserAsync(int topicId, string userId, string moderatorId, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Empty, Error>> DeassignModerator(int topicId, string userId, DateTime? schedule = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Empty, Error>> HideBlogAsync(int blogId, string moderatorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Empty, Error>> HideCommentAsync(int commentId, string moderatorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<int, Error>> HidePost<TPost>(int id, string moderatorId) where TPost : Post
        {
            throw new NotImplementedException();
        }

        public Task<Result<Empty, Error>> UnbanUserAsync(int topicId, string userId, string moderatorId)
        {
            throw new NotImplementedException();
        }
    }
}