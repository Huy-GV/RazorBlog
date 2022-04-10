using BlogApp.Data.DTOs;
using BlogApp.Services.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<IList<CommentDto>> GetAllCommentsAsync(int blogId);
        public Task<ServiceResult> CreateCommentAsync(int blogId);
        public Task<ServiceResult> UpdateCommentAsync();
        public Task<ServiceResult> DeleteCommentAsync(int commentId);
    }
}
