using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorBlog.Data.ViewModels;
using RazorBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Authentication
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly Services.Interfaces.IAuthenticationService _authenticationService;

        public RegisterModel(
            SignInManager<ApplicationUser> signInManager,
            Services.Interfaces.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [BindProperty]
        public RegisterViewModel CreateUserViewModel { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Register(CreateUserViewModel);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }

            return Page();
        }
    }
}