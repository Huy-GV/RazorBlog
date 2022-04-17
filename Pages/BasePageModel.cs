using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Models;

namespace RazorBlog.Pages
{
    public class BasePageModel<TPageModel> : PageModel where TPageModel : PageModel
    {
        protected RazorBlogDbContext DbContext { get; }
        protected UserManager<ApplicationUser> UserManager { get; }
        protected ILogger<TPageModel> Logger { get; }

        public BasePageModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<TPageModel> logger)
        {
            DbContext = context;
            UserManager = userManager;
            Logger = logger;
        }
    }
}