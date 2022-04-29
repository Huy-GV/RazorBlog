namespace RazorBlog.Services.Interfaces
{
    using RazorBlog.Data.DTOs;
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Services.Communications;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentService
    {
        public Task<IList<CommentDto>> GetAllCommentsAsync(int blogId);

        public Task<Result<int, Error>> CreateCommentAsync(int blogId, CreateCommentViewModel commentViewModel);

        public Task<Result<Empty, Error>> UpdateCommentAsync(int commentId, int blogId);

        public Task<Result<Empty, Error>> DeleteCommentAsync(int commentId);
    }
}