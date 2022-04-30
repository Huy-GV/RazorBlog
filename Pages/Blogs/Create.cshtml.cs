using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Data.ViewModels;
using RazorBlog.Services.Interfaces;
using RazorBlog.Models;
using System;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Blogs
{
    [Authorize]
    public class CreateModel : BasePageModel<CreateModel>
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public BlogViewModel CreateBlogViewModel { get; set; }

        public CreateModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<CreateModel> logger,
            IBlogService blogService) : base(
                context, userManager, logger)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated.Equals(false) ?? true)
            {
                return Challenge();
            }

            return Page();
        }

        // todo: make topic a select/ radio group
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state when submitting new post");
                return Page();
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var result = await _blogService.CreateBlogAsync(CreateBlogViewModel, userId);
                if (result.Succeeded)
                {
                    return RedirectToPage("Read", new { id = result.Data });
                }

                return Page();
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to create blog");
                Logger.LogError(ex.Message);

                return Page();
            }
        }
    }
}