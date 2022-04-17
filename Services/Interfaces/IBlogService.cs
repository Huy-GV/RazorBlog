using RazorBlog.Data.DTOs;
using RazorBlog.Services.Communications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorBlog.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IList<BlogDto>> GetAllBlogsAsync(string userId);

        Task<DetailedBlogDto> GetBlogByIdAsync(int blogId);

        Task<Result> CreateBlogAsync();

        Task<Result> HidePostAsync();

        Task<Result> UpdateBlogAsync(int blogId);

        Task<Result> DeleteBlogAsync(int blogId);
    }
}