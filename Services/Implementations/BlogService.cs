namespace RazorBlog.Services.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using RazorBlog.Data;
    using RazorBlog.Data.DTOs;
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Models;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BlogService : IBlogService
    {
        private readonly IImageService _imageService;
        private readonly RazorBlogDbContext _context;
        private ILogger<BlogService> _logger;
        private IAuthenticationService _authenticationService;

        public BlogService(IImageService imageService,
            RazorBlogDbContext context,
            ILogger<BlogService> logger,
            IAuthenticationService authenticationService)
        {
            _imageService = imageService;
            _context = context;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        public async Task<Result<int, Error>> CreateBlogAsync(BlogViewModel viewModel, string? userId)
        {
            if (!await _authenticationService.UserExists(userId))
            {
                // todo: configure default data for int (avoid 0)?
                return ResultUtil.Failure<int>(ServiceCode.NotFound, $"User with ID {userId} was not found.");
            }

            //var option = new TransactionOptions() { IsolationLevel = IsolationLevel.RepeatableRead };
            //using var transactionScope = new TransactionScope(TransactionScopeOption.Required, option);
            try
            {
                var imageUri = await _imageService.UploadBlogCoverImageAsync(viewModel.CoverImage);
                var topic = await _context.Topic.SingleAsync(t => t.Name == viewModel.TopicName);
                var newBlog = new Blog
                {
                    Title = viewModel.Title,
                    Content = viewModel.Content,
                    TopicId = topic.Id,
                    CoverImageUri = imageUri,
                    Introduction = viewModel.Introduction,
                    Date = DateTime.Now,
                    AppUserId = userId,
                };

                _context.Blog.Add(newBlog);
                await _context.SaveChangesAsync();
                // transactionScope.Complete();
                return ResultUtil.Success(newBlog.Id);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception message: {exception.Message}");
                // transactionScope.Dispose();
                return ResultUtil.Failure<int>(ServiceCode.InternalError);
            }
        }

        public async Task<IList<BlogDto>> GetAllBlogsAsync(string? searchString)
        {
            // todo: include sorts by views, post date, trending (view count after a number of days)
            searchString = searchString?.Trim().Trim(' ') ?? string.Empty;
            var blogs = await _context.Blog
                .Include(b => b.AppUser)
                .Include(b => b.Comments)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.Topic)
                .Select(b => new BlogDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author,
                    CreatedDate = b.Date,
                    TopicName = b.Topic.Name,
                    ViewCount = b.ViewCount,
                    Date = b.Date,
                    CoverImageUri = b.CoverImageUri,
                    Introduction = b.Introduction,
                })
                .Where(b => searchString == null ||
                        searchString == string.Empty ||
                        b.Title.Contains(searchString) ||
                        b.AuthorName.Contains(searchString) ||
                        b.TopicName.Contains(searchString))
                .Take(10)
                .ToListAsync();

            return blogs;
        }

        public async Task<Result<DetailedBlogDto, Error>> GetBlogByIdAsync(int blogId)
        {
            var blog = await _context.Blog
                .Include(b => b.Comments)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.AppUser)
                .SingleOrDefaultAsync(b => b.Id == blogId);

            if (blog == null)
            {
                return ResultUtil.Failure<DetailedBlogDto>(ServiceCode.NotFound, $"No blog with ID {blogId} was found.");
            }

            await IncrementViewCount(blog);

            var blogAuthor = new
            {
                UserName = blog.AppUser?.UserName ?? "Deleted User",
                // todo: un-hardcode default profile pic
                ProfileImageUri = blog.AppUser?.ProfileImageUri ?? "default.jpg",
                Description = blog.AppUser?.Description ?? "Deleted User"
            };

            return ResultUtil.Success(new DetailedBlogDto
            {
                Id = blogId,
                Introduction = blog.Introduction,
                Content = blog.Content,
                CoverImageUri = blog.CoverImageUri,
                Date = blog.Date,
                IsHidden = blog.IsHidden,
                AuthorDescription = blogAuthor.Description,
                AuthorName = blogAuthor.UserName,
                AuthorProfileImageUri = blogAuthor.ProfileImageUri,
                CommentDtos = blog.Comments
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Date = c.Date,
                        Content = c.Content,
                        AuthorName = c.AppUser?.UserName ?? "Deleted User",
                        AuthorProfilePicturePath = c.AppUser?.ProfileImageUri ?? "default.jpg",
                        IsHidden = c.IsHidden,
                    })
                    .ToList()
            });
        }

        private async Task IncrementViewCount(Blog blog)
        {
            blog.ViewCount++;
            _context.Blog.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<Result<Empty, Error>> UpdateBlogAsync(int blogId, BlogViewModel viewModel, string userId)
        {
            var blog = await _context.Blog
                .Include(b => b.AppUser)
                .SingleAsync(b => b.Id == blogId);

            if (blog == null)
            {
                return ResultUtil.Failure(ServiceCode.NotFound, $"No blog with ID {blogId} was found.");
            }

            if (userId != blog.AppUser?.Id)
            {
                return ResultUtil.Failure(ServiceCode.UnauthorizedAction);
            }

            if (viewModel.CoverImage != null)
            {
                try
                {
                    var deleteResult = _imageService.DeleteImage(
                        blog.CoverImageUri,
                        Data.Constants.ImageType.BlogCover);
                    if (!deleteResult.Succeeded)
                    {
                        return ResultUtil.Failure(ServiceCode.InternalError, "Failed to delete old cover image.");
                    }

                    blog.CoverImageUri = await _imageService.UploadBlogCoverImageAsync(viewModel.CoverImage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to update cover image of blog with ID {blogId}.");
                    _logger.LogError(ex.Message);
                }
            }

            _context.Blog.Update(blog).CurrentValues.SetValues(viewModel);
            await _context.SaveChangesAsync();
            return ResultUtil.Success();
        }

        public async Task<Result<Empty, Error>> DeleteBlogAsync(string userId, int blogId)
        {
            var blog = await _context.Blog
                .Include(b => b.AppUser)
                .SingleAsync(b => b.Id == blogId);

            if (blog == null)
            {
                return ResultUtil.Failure(ServiceCode.NotFound, $"No blog with ID {blogId} was found.");
            }

            if (userId != blog.AppUser?.Id)
            {
                return ResultUtil.Failure(ServiceCode.UnauthorizedAction);
            }

            var imageDeleteResult = _imageService.DeleteImage(blog.CoverImageUri, Data.Constants.ImageType.BlogCover);
            if (!imageDeleteResult.Succeeded)
            {
                return ResultUtil.Failure(ServiceCode.InternalError, imageDeleteResult.Error.Message);
            }

            _context.Remove(blog);
            await _context.SaveChangesAsync();

            return ResultUtil.Success();
        }

        public async Task<bool> BlogExists(int blogId)
            => await _context.Blog.AnyAsync(b => b.Id == blogId);
    }
}