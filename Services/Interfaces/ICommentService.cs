namespace RazorBlog.Services.Interfaces
{
    using RazorBlog.Data.DTOs;
    using RazorBlog.Data.ViewModel;
    using RazorBlog.Services.Communications;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentService
    {
        public Task<IList<CommentDto>> GetAllCommentsAsync(int blogId);

        public Task<Result> CreateCommentAsync(int blogId, CreateCommentViewModel commentViewModel);

        public Task<Result> UpdateCommentAsync(int commentId, int blogId);

        public Task<Result> DeleteCommentAsync(int commentId);
    }
}