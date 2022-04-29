using RazorBlog.Services.Communications;
using System;
using System.Threading.Tasks;

namespace RazorBlog.Services.Interfaces
{
    public interface IModerationService
    {
        public Task<Result<Empty, Error>> HideCommentAsync(int commentId, string moderatorId);

        public Task<Result<Empty, Error>> HideBlogAsync(int blogId, string moderatorId);

        /// <summary>
        /// Ban a user from making post on a topic temporarily or permanently.
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <param name="moderatorId"></param>
        /// <param name="endDate">The date when the ban is lifted. Leave this field null to ban a user permanently.</param>
        /// <returns></returns>
        public Task<Result<Empty, Error>> BanUserAsync(
            int topicId,
            string userId,
            string moderatorId,
            DateTime? endDate = null);

        // todo: use quartz for scheduled unban
        /// <summary>
        /// Unban a user from posting in a topic immediately
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <param name="moderatorId"></param>
        /// <returns></returns>
        public Task<Result<Empty, Error>> UnbanUserAsync(
            int topicId,
            string userId,
            string moderatorId);

        /// <summary>
        /// Promote a user to a moderator role for a topic.
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <param name="schedule">The scheduled date when the change occurs. Set to null to promote this user immediately.</param>
        /// <returns></returns>
        public Task<Result<Empty, Error>> AssignModerator(
            int topicId,
            string userId,
            DateTime? schedule = null);

        /// <summary>
        /// Deassign a user from the moderator role of a topic.
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <param name="schedule">The scheduled date when the change occurs. Set to null to demote this user immediately.</param>
        /// <returns></returns>
        public Task<Result<Empty, Error>> DeassignModerator(
            int topicId,
            string userId,
            DateTime? schedule = null);
    }
}