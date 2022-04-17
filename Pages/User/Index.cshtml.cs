using RazorBlog.Data;
using RazorBlog.Data.DTOs;
using RazorBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RazorBlog.Pages.User
{
    [Authorize]
    public class IndexModel : BasePageModel<IndexModel>
    {
        [BindProperty]
        public PersonalProfileDto UserDto { get; set; }

        public IndexModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger) : base(context, userManager, logger)
        {
        }

        public async Task<IActionResult> OnGetAsync(string? username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserName != User.Identity.Name)
            {
                return Forbid();
            }

            var blogs = DbContext.Blog
                .Include(b => b.AppUser)
                .AsNoTracking()
                .Where(blog => blog.AppUser.UserName == username)
                .ToList();

            UserDto = new PersonalProfileDto()
            {
                UserName = username,
                BlogCount = blogs.Count,
                ProfilePicturePath = user.ProfilePicturePath,
                Blogs = blogs,
                Description = user.Description,
                CommentCount = DbContext.Comment
                    .Where(comment => comment.AppUser.UserName == username)
                    .ToList()
                    .Count,
                BlogCountCurrentYear = blogs
                    .Where(blog => blog.Author == username && blog.Date.Year == DateTime.Now.Year)
                    .ToList()
                    .Count,
                ViewCountCurrentYear = blogs
                    .Where(blog => blog.Author == username && blog.Date.Year == DateTime.Now.Year)
                    .Sum(blogs => blogs.ViewCount),
                Country = user.Country,
                RegistrationDate = user.RegistrationDate?.ToString("dd MM yyyy") ?? "",
            };

            return Page();
        }
    }
}