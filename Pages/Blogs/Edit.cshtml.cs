using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Data.ViewModels;
using RazorBlog.Models;
using RazorBlog.Services;

namespace RazorBlog.Pages.Blogs;

[Authorize]
public class EditModel : BasePageModel<EditModel>
{
    private readonly IImageStorage _imageStorage;

    public EditModel(
        RazorBlogDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<EditModel> logger,
        IImageStorage imageStorage) : base(context, userManager, logger)
    {
        _imageStorage = imageStorage;
    }

    [BindProperty] public EditBlogViewModel EditBlogViewModel { get; set; }

    public async Task<IActionResult> OnGetAsync(int? blogId, string? username)
    {
        if (blogId == null || username == null) return NotFound();

        if (User.Identity?.Name != username) return Forbid();

        var blog = await DbContext.Blog.FindAsync(blogId);

        if (blog == null) return NotFound();

        EditBlogViewModel = new EditBlogViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            Description = blog.Introduction
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

        var user = await GetUserAsync();
        var blog = await DbContext.Blog.FindAsync(EditBlogViewModel.Id);

        if (blog == null) return NotFound();

        if (string.IsNullOrEmpty(EditBlogViewModel.Content)) return RedirectToPage("/Blogs/Read", new { id = blog.Id });

        if (user.UserName != blog.AppUser.UserName) return Forbid();

        DbContext.Blog.Update(blog).CurrentValues.SetValues(EditBlogViewModel);

        if (EditBlogViewModel.CoverImage != null)
            try
            {
                await _imageStorage.DeleteImage(blog.CoverImageUri);
                var imageName = await _imageStorage.UploadBlogCoverImageAsync(EditBlogViewModel.CoverImage);
                blog.CoverImageUri = imageName;
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to update blog image");
                Logger.LogError(ex.Message);
            }

        await DbContext.SaveChangesAsync();
        return RedirectToPage("/Blogs/Read", new { id = blog.Id });
    }
}