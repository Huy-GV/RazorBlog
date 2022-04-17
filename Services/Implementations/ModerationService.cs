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

        public Task<Result> AssignModerator(int topicId, string userId, DateTime? schedule = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result> BanUserAsync(int topicId, string userId, string moderatorId, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeassignModerator(int topicId, string userId, DateTime? schedule = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result> HideBlogAsync(int blogId, string moderatorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> HideCommentAsync(int commentId, string moderatorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UnbanUserAsync(int topicId, string userId, string moderatorId)
        {
            throw new NotImplementedException();
        }
    }
}