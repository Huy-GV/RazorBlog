using BlogApp.Data.DTOs;
using BlogApp.Services.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface IBlogService
    {
        public Task<IList<DetailedBlogDto>> GetAllBlogsAsync(string userId);
        public Task<DetailedBlogDto> GetBlogByIdAsync(int blogId);
        public Task<ServiceResult> CreateBlogAsync();
        public Task<ServiceResult> UpdateBlogAsync();
        public Task<ServiceResult> DeleteBlogAsync(int blogId);
    }
}
