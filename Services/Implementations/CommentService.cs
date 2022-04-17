namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Data.DTOs;
    using RazorBlog.Data.ViewModel;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
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