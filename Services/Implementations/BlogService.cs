namespace RazorBlog.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
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
    using System.Transactions;

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

        public async Task<Result> CreateBlogAsync(BlogViewModel viewModel, string? userId)
        {
            if (!await _authenticationService.UserExists(userId))
            {
                return new Result
                {
                    Code = ServiceCode.InvalidArgument,
                    ErrorMessage = "User ID must not be null or empty."
                };
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
                    Introduction = viewModel.Description,
                    Date = DateTime.Now,
                    AppUserId = userId,
                };

                _context.Blog.Add(newBlog);
                await _context.SaveChangesAsync();
                // transactionScope.Complete();
                return new Result { Code = ServiceCode.Success };
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception message: {exception.Message}");
                // transactionScope.Dispose();
                return new Result { Code = ServiceCode.InternalError };
            }
        }

        public Task<Result> DeleteBlogAsync(int blogId)
        {
            throw new System.NotImplementedException();
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
                .Where(b => (searchString == null || searchString == string.Empty) ||
                        b.Title.Contains(searchString) ||
                        b.AuthorName.Contains(searchString) ||
                        b.TopicName.Contains(searchString))
                .Take(10)
                .ToListAsync();

            return blogs;
        }

        public Task<IList<BlogDto>> GetAllBlogsInSubscribedTopicAsync(string? userId, string? searchString)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DetailedBlogDto> GetBlogByIdAsync(int blogId)
        {
            var blog = await _context.Blog
                .SingleOrDefaultAsync(b => b.Id == blogId);

            if (blog == null)
            {
            }

            await IncrementViewCount(blog);
        }

        private async Task IncrementViewCount(Blog blog)
        {
            blog.ViewCount++;
            _context.Blog.Update(blog);
            await _context.SaveChangesAsync();
        }

        public Task<Result> HidePostAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> UpdateBlogAsync(int blogId)
        {
            throw new System.NotImplementedException();
        }
    }
}