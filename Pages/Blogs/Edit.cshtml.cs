using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Data.ViewModel;
using RazorBlog.Interfaces;
using RazorBlog.Models;
using System;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Blogs
{
    [Authorize]
    public class EditModel : BasePageModel<EditModel>
    {
        [BindProperty]
        public EditBlogViewModel EditBlogVM { get; set; }

        private readonly IImageService _imageService;

        public EditModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<EditModel> logger,
            IImageService imageFileService) : base(context, userManager, logger)
        {
            _imageService = imageFileService;
        }

        public async Task<IActionResult> OnGetAsync(int? blogID, string? username)
        {
            if (blogID == null || username == null)
                return NotFound();

            if (User.Identity.Name != username)
            {
                return Unauthorized();
            }

            var blog = await DbContext.Blog.FindAsync(blogID);

            EditBlogVM = new EditBlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Description = blog.Description
            };

            return Page();
        }

        public async Task<IActionResult> OnPostEditBlogAsync()
        {
            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state when editing blog");
                return Page();
            }

            var user = await UserManager.GetUserAsync(User);
            var blog = await DbContext.Blog.FindAsync(EditBlogVM.Id);

            if (blog == null)
                return NotFound();
            if (EditBlogVM.Content == "")
                return RedirectToPage("/Blogs/Read", new { id = blog.Id });
            if (user.UserName != blog.Author)
                return Forbid();

            DbContext.Blog.Attach(blog).CurrentValues.SetValues(EditBlogVM);

            if (EditBlogVM.CoverImage != null)
            {
                try
                {
                    _imageService.DeleteImage(blog.CoverImageUri);
                    var imageFile = EditBlogVM.CoverImage;
                    var imageName = _imageService.BuildFileName(imageFile.FileName);
                    blog.CoverImageUri = imageName;

                    await _imageService.UploadBlogImageAsync(imageFile, imageName);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Failed to update blog image");
                    Logger.LogError(ex.Message);
                }
            }

            DbContext.Attach(blog).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return RedirectToPage("/Blogs/Read", new { id = blog.Id });
        }
    }
}