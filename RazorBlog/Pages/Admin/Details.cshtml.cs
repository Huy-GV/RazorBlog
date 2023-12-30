using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Data.Constants;
using RazorBlog.Data.Dtos;
using RazorBlog.Models;
using RazorBlog.Services;

namespace RazorBlog.Pages.Admin;

[Authorize(Roles = Roles.AdminRole)]
public class DetailsModel(
    RazorBlogDbContext context,
    UserManager<ApplicationUser> userManager,
    ILogger<DetailsModel> logger,
    IUserModerationService userUserModerationService,
    IPostDeletionService postDeletionService) : BasePageModel<DetailsModel>(context, userManager, logger)
{
    private readonly IUserModerationService _userModerationService = userUserModerationService;
    private readonly IPostDeletionService _postDeletionService = postDeletionService;

    [BindProperty] public BanTicket? BanTicket { get; set; }
    [BindProperty] public string UserName { get; set; } = string.Empty;
    [BindProperty] public List<HiddenCommentDto> HiddenComments { get; set; } = [];
    [BindProperty] public List<HiddenBlogDto> HiddenBlogs { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(string? userName)
    {
        if (userName == null)
        {
            return NotFound();
        }

        var user = await UserManager.FindByNameAsync(userName);
        if (user == null)
        {
            Logger.LogInformation("User not found");
            return NotFound();
        }

        UserName = userName;
        HiddenComments = await GetHiddenComments(userName);
        HiddenBlogs = await GetHiddenBlogs(userName);
        BanTicket = await _userModerationService.FindByUserNameAsync(userName);

        return Page();
    }

    private async Task<List<HiddenBlogDto>> GetHiddenBlogs(string username)
    {
        return await DbContext.Blog
            .Include(b => b.AppUser)
            .Where(b => b.AppUser.UserName == username && b.IsHidden)
            .Select(b => new HiddenBlogDto
            {
                Id = b.Id,
                Title = b.Title,
                Introduction = b.Introduction,
                Content = b.Content,
                CreationTime = b.CreationTime,
            })
            .ToListAsync();
    }

    private async Task<List<HiddenCommentDto>> GetHiddenComments(string username)
    {
        return await DbContext.Comment
            .Include(c => c.AppUser)
            .Where(c => c.AppUser.UserName == username && c.IsHidden)
            .Select(c => new HiddenCommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreationTime = c.CreationTime,
            })
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostBanUserAsync()
    {
        if (BanTicket?.UserName == null || await UserManager.FindByNameAsync(BanTicket.UserName) == null)
        {
            return BadRequest("User not found");
        }

        await _userModerationService.BanUserAsync(BanTicket.UserName, BanTicket.Expiry);

        return RedirectToPage("Details", new { username = BanTicket.UserName });
    }

    public async Task<IActionResult> OnPostLiftBanAsync(string userName)
    {
        if (!await _userModerationService.BanTicketExistsAsync(userName))
        {
            return BadRequest();
        }

        await _userModerationService.RemoveBanTicketAsync(userName);

        return RedirectToPage("Details", new { userName });
    }

    public async Task<IActionResult> OnPostUnhideBlogAsync(int blogId)
    {
        var blog = await DbContext.Blog
            .Include(b => b.AppUser)
            .FirstOrDefaultAsync(b => b.Id == blogId);

        if (blog == null)
        {
            Logger.LogError("blog not found");
            return NotFound();
        }

        DbContext.Blog.Update(blog);
        blog.IsHidden = false;
        await DbContext.SaveChangesAsync();

        return RedirectToPage("Details", new { username = blog.AppUser.UserName });
    }

    public async Task<IActionResult> OnPostUnhideCommentAsync(int commentId)
    {
        var comment = await DbContext.Comment
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null)
        {
            Logger.LogError("Comment not found");
            return NotFound();
        }

        DbContext.Comment.Update(comment);
        comment.IsHidden = false;
        await DbContext.SaveChangesAsync();

        return RedirectToPage("Details", new { username = comment.AppUser.UserName });
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId)
    {
        var comment = await DbContext.Comment
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null)
        {
            Logger.LogError("Comment not found");
            return NotFound();
        }

        DbContext.Comment.Update(comment);
        comment.IsHidden = false;
        comment.Content = ReplacementText.RemovedContent;
        comment.ToBeDeleted = true;
        await DbContext.SaveChangesAsync();
        var deleteTime = new DateTimeOffset(DateTime.UtcNow.AddDays(7));
        _postDeletionService.ScheduleBlogDeletion(deleteTime, comment.Id);

        return RedirectToPage("Details", new { username = comment.AppUser.UserName });
    }

    public async Task<IActionResult> OnPostDeleteBlogAsync(int blogId)
    {
        var blog = await DbContext.Blog
            .Include(b => b.AppUser)
            .FirstOrDefaultAsync(b => b.Id == blogId);

        if (blog == null)
        {
            Logger.LogError("Blog not found");
            return NotFound();
        }

        DbContext.Blog.Update(blog);
        blog.IsHidden = false;
        blog.ToBeDeleted = true;
        blog.Title = ReplacementText.RemovedContent;
        blog.Introduction = ReplacementText.RemovedContent;
        blog.Content = ReplacementText.RemovedContent;
        await DbContext.SaveChangesAsync();

        var deleteTime = new DateTimeOffset(DateTime.UtcNow.AddDays(14));
        _postDeletionService.ScheduleBlogDeletion(deleteTime, blog.Id);
        return RedirectToPage("Details", new { username = blog.AppUser.UserName });
    }
}