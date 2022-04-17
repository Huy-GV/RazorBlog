using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorBlog.Models;
using RazorBlog.Data.ViewModel;
using RazorBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace RazorBlog.Pages.Authentication
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly Services.Interfaces.IAuthenticationService _authenticationService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            Services.Interfaces.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [BindProperty]
        public SignInViewModel LogInViewModel { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            Console.WriteLine("Return URL from log in model: " + returnUrl);

            if (ModelState.IsValid)
            {
                var result = await _authenticationService.SignIn(LogInViewModel);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError("AuthError", result.ErrorMessage);
                return Page();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}