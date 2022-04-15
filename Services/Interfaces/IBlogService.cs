using BlogApp.Data.DTOs;
using BlogApp.Services.Communications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
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