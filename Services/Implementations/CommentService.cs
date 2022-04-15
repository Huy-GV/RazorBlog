namespace BlogApp.Services.Implementations
{
    using BlogApp.Data.DTOs;
    using BlogApp.Data.ViewModel;
    using BlogApp.Services.Communications;
    using BlogApp.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        public Task<Result> CreateCommentAsync(int blogId, CreateCommentViewModel commentViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> DeleteCommentAsync(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<CommentDto>> GetAllCommentsAsync(int blogId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> UpdateCommentAsync(int commentId, int blogId)
        {
            throw new System.NotImplementedException();
        }
    }
}