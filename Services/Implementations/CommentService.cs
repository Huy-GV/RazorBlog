namespace RazorBlog.Services.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using RazorBlog.Data;
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly IBlogService _blogService;
        private readonly RazorBlogDbContext _context;
        private readonly IAuthenticationService _authenticationService;

        public CommentService(
            IBlogService blogService,
            RazorBlogDbContext context,
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _context = context;
            _blogService = blogService;
        }

        public async Task<Result<int, Error>> CreateCommentAsync(CommentViewModel viewModel, string userId)
        {
            if (!await _authenticationService.UserExists(userId))
            {
                return ResultUtil.Failure<int>(ServiceCode.NotFound, $"No user with ID {userId} was found.");
            }

            try
            {
                var newComment = new Models.Comment
                {
                    AppUserId = userId,
                    Date = System.DateTime.Now,
                    Content = viewModel.CommentContent,
                    BlogId = viewModel.BlogId,
                };

                _context.Comment.Add(newComment);
                await _context.SaveChangesAsync();

                return ResultUtil.Success(newComment.Id);
            }
            catch
            {
                return ResultUtil.Failure<int>(ServiceCode.InternalError);
            }
        }

        public async Task<Result<int, Error>> DeleteCommentAsync(int commentId, string userId)
        {
            var comment = await _context.Comment
                .Include(c => c.AppUser)
                .SingleAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return ResultUtil.Failure<int>(ServiceCode.NotFound);
            }

            if (userId != comment.AppUser?.Id)
            {
                return ResultUtil.Failure<int>(ServiceCode.UnauthorizedAction);
            }

            var blogId = comment.BlogId;
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return ResultUtil.Success(blogId);
        }

        public async Task<Result<Empty, Error>> UpdateCommentAsync(int commentId, string userId, CommentViewModel viewModel)
        {
            if (!await _blogService.BlogExists(viewModel.BlogId))
            {
                return ResultUtil.Failure(ServiceCode.NotFound);
            }

            var comment = await _context.Comment
                .Include(c => c.AppUser)
                .SingleAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return ResultUtil.Failure(ServiceCode.NotFound);
            }

            if (userId != comment.AppUser?.Id)
            {
                return ResultUtil.Failure(ServiceCode.UnauthorizedAction);
            }

            _context.Comment.Update(comment).CurrentValues.SetValues(viewModel);
            await _context.SaveChangesAsync();

            return ResultUtil.Success();
        }
    }
}