namespace RazorBlog.Services.Interfaces
{
    using RazorBlog.Data.DTOs;
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Services.Communications;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentService
    {
        public Task<Result<int, Error>> CreateCommentAsync(CommentViewModel commentViewModel, string userId);

        public Task<Result<Empty, Error>> UpdateCommentAsync(int commentId, string userId, CommentViewModel viewModel);

        public Task<Result<int, Error>> DeleteCommentAsync(int commentId, string userId);
    }
}