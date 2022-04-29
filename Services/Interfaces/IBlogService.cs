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

        Task<Result<DetailedBlogDto, Error>> GetBlogByIdAsync(int blogId);

        Task<Result<int, Error>> CreateBlogAsync(BlogViewModel viewModel, string? userId);

        Task<Result<Empty, Error>> HidePostAsync();

        Task<Result<int, Error>> UpdateBlogAsync(int blogId);

        Task<Result<Empty, Error>> DeleteBlogAsync(int blogId);
    }
}