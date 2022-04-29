namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Data.DTOs;
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        public Task<Result<int, Error>> CreateCommentAsync(int blogId, CreateCommentViewModel commentViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Empty, Error>> DeleteCommentAsync(int commentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<IList<CommentDto>, Error>> GetAllCommentsAsync(int blogId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<Empty, Error>> UpdateCommentAsync(int commentId, int blogId)
        {
            throw new System.NotImplementedException();
        }

        Task<IList<CommentDto>> ICommentService.GetAllCommentsAsync(int blogId)
        {
            throw new System.NotImplementedException();
        }
    }
}