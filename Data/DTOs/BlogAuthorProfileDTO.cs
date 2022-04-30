using Microsoft.AspNetCore.Identity;
using RazorBlog.Models;
using System;
using System.Threading.Tasks;

namespace RazorBlog.Data.DTOs
{
    [Obsolete]
    public class BlogAuthorProfileDto : BaseProfileDto
    {
        public string Description { get; set; } = "None";

        public static async Task<BlogAuthorProfileDto> From(
            UserManager<ApplicationUser> userManager,
            string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new BlogAuthorProfileDto()
                {
                    Description = "Deleted",
                    UserName = userName
                };
            }

            return new BlogAuthorProfileDto()
            {
                Description = user.Description,
                ProfilePicturePath = user.ProfileImageUri,
                UserName = user.UserName
            };
        }
    }
}