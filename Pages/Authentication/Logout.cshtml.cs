using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Authentication
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public LogoutModel(ILogger<LogoutModel> logger)
        {
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToPage("/Blogs/Index");
        }
    }
}