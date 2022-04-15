namespace BlogApp.Services.Implementations
{
    using BlogApp.Data.DTOs;
    using BlogApp.Services.Communications;
    using BlogApp.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BlogService : IBlogService
    {
        private readonly IImageService _imageService;

        public Task<Result> CreateBlogAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> DeleteBlogAsync(int blogId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<BlogDto>> GetAllBlogsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<DetailedBlogDto> GetBlogByIdAsync(int blogId)
        {
            throw new System.NotImplementedException();
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