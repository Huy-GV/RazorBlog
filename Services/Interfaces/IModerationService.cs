using BlogApp.Services.Results;
using System;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface IModerationService
    {
        public Task<ServiceResult> HideCommentAsync(int commentId, string moderatorId);
        public Task<ServiceResult> HideBlogAsync(int blogId, string moderatorId);
        public Task<ServiceResult> BanUserAsync(
            int communityId, 
            string userId, 
            string moderatorId,
            DateTime? endDate = null);
        public Task<ServiceResult> UnBanUserAsync(
            int communityId,
            string userId,
            string moderatorId);
        public Task<ServiceResult> AssignModerator(
            int communityId,
            string topModeratorId,
            string userId);
        public Task<ServiceResult> DeassignModerator(
            int communityId,
            string topModeratorId,
            string userId);
    }
}
