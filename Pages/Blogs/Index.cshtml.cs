using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Identity;
using BlogApp.Pages;
using Microsoft.Extensions.Logging;

namespace BlogApp.Pages.Blogs
{
    [AllowAnonymous]
    public class IndexModel : BasePageModel<IndexModel>
    {
        public IEnumerable<Blog> Blogs { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public IndexModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger) : base(
                context, userManager, logger)
        {
        }

        public async Task OnGetAsync()
        {
            var blogs = DbContext.Blog
                .Include(b => b.AppUser)
                // .Include(b => b.Topic)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower().Trim();
                blogs = blogs.Where(b =>
                    b.AppUser.UserName.Contains(SearchString) ||
                    // b.Topic.Name.Contains(SearchString) ||
                    b.Title.Contains(SearchString));
            }

            Blogs = await blogs.ToListAsync();
        }
    }
}