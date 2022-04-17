using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Blogs
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