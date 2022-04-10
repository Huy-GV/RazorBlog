using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using BlogApp.Services;
using BlogApp.Data.ViewModel;
using BlogApp.Interfaces;

namespace BlogApp.Pages.Blogs
{
    [Authorize]
    public class CreateModel : BasePageModel<CreateModel>
    {
        [BindProperty]
        public CreateBlogViewModel CreateBlogVM { get; set; }
        private readonly IImageService _imageService;
        public CreateModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<CreateModel> logger,
            IImageService imageService) : base(
                context, userManager, logger)
        {
            _imageService = imageService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await UserManager.GetUserAsync(User);
            var username = user.UserName;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await UserManager.GetUserAsync(User);
            var username = user.UserName;

            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state when submitting new post");
                return Page();
            }

            try
            {
                var coverImage = CreateBlogVM.CoverImage;
                var imageName = _imageService.BuildFileName(coverImage.FileName);
                await _imageService.UploadBlogImageAsync(coverImage, imageName);
                var entry = DbContext.Blog.Add(new Blog()
                {
                    ImagePath = imageName,
                    Date = DateTime.Now,
                    AppUserId = user.Id
                });

                entry.CurrentValues.SetValues(CreateBlogVM);
                await DbContext.SaveChangesAsync();
                
                return RedirectToPage("./Index");
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

