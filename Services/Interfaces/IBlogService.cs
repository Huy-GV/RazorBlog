using RazorBlog.Data.DTOs;
using RazorBlog.Data.ViewModels;
using RazorBlog.Services.Communications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorBlog.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IList<BlogDto>> GetAllBlogsAsync(string? searchString);

        Task<IList<BlogDto>> GetAllBlogsInSubscribedTopicAsync(string? userId, string? searchString);

        Task<DetailedBlogDto> GetBlogByIdAsync(int blogId);

        Task<Result> CreateBlogAsync(BlogViewModel viewModel, string? userId);

        Task<Result> HidePostAsync();

        Task<Result> UpdateBlogAsync(int blogId);

        Task<Result> DeleteBlogAsync(int blogId);
    }
}