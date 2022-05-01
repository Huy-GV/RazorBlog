using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Data.Constants;
using RazorBlog.Data.DTOs;
using RazorBlog.Data.ViewModels;
using RazorBlog.Models;
using RazorBlog.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Blogs
{
    [AllowAnonymous]
    public class ReadModel : BasePageModel<ReadModel>
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;

        [BindProperty]
        public CommentViewModel CreateCommentViewModel { get; set; }

        [BindProperty]
        public CommentViewModel EditCommentViewModel { get; set; }

        public Blog Blog { get; set; }
        public DetailedBlogDto DetailedBlogDto { get; set; }

        public ReadModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ReadModel> logger,
            IBlogService blogService,
            ICommentService commentService) : base(
                context, userManager, logger)
        {
            _commentService = commentService;
            _blogService = blogService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Blog = await DbContext.Blog
            //    .Include(blog => blog.AppUser)
            //    .Include(blog => blog.Comments)
            //        .ThenInclude(comment => comment.AppUser)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(blog => blog.Id == id);

            //if (Blog == null)
            //{
            //    return NotFound();
            //}

            //await IncrementViewCountAsync(id.Value);
            //ViewData["IsSuspended"] = false;

            //DetailedBlogDto = DetailedBlogDto.From(Blog);

            // get this from moderation service?
            ViewData["IsSuspended"] = false;
            var result = await _blogService.GetBlogByIdAsync(id.Value);
            if (result.Succeeded)
            {
                DetailedBlogDto = result.Data;
                return Page();
            }

            if (result.Code == Services.Communications.ServiceCode.NotFound)
            {
                return NotFound();
            }

            // todo: create a viewmodel that dictatates whether a user can make changes/ comments

            return Page();
        }

        public async Task<IActionResult> OnPostCreateCommentAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            // todo: modelstate adds a content field??
            //if (!ModelState.IsValid)
            //{
            //    Logger.LogError("Model state invalid when submitting comments");
            //    return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
            //}

            // todo: avoid using UM here?
            var user = await UserManager.GetUserAsync(User);
            var result = await _commentService.CreateCommentAsync(CreateCommentViewModel, user.Id);
            if (result.Succeeded)
            {
                return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
            }

            if (result.Code == Services.Communications.ServiceCode.NotFound)
            {
                return NotFound();
            }

            return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
        }

        public async Task<IActionResult> OnPostEditCommentAsync(int commentID)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            //if (!ModelState.IsValid)
            //{
            //    Logger.LogError("Model state invalid when editting comments");
            //    return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
            //}

            var userId = await GetUserId();
            var result = await _commentService.UpdateCommentAsync(commentID, userId, CreateCommentViewModel);
            if (result.Succeeded)
            {
                return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
            }

            if (result.Code == Services.Communications.ServiceCode.NotFound)
            {
                return NotFound();
            }

            return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
        }

        [Obsolete]
        public async Task<IActionResult> OnPostHideBlogAsync(int blogID)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            var user = await UserManager.GetUserAsync(User);
            var roles = await UserManager.GetRolesAsync(user);

            if (!(roles.Contains(Roles.AdminRole) ||
                roles.Contains(Roles.ModeratorRole)))
                return Forbid();

            var blog = await DbContext.Blog.FindAsync(blogID);
            if (blog == null)
                return NotFound();

            if (blog.Author == "admin")
                return Forbid();

            // blog.SuspensionExplanation = Messages.InappropriateBlog;
            // await DbContext.SaveChangesAsync();

            return RedirectToPage("/Blogs/Read", new { id = blogID });
        }

        [Obsolete]
        public async Task<IActionResult> OnPostHideCommentAsync(int commentID)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            var user = await UserManager.GetUserAsync(User);
            var roles = await UserManager.GetRolesAsync(user);
            if (!(roles.Contains(Roles.AdminRole)
                || roles.Contains(Roles.ModeratorRole)))
                return Forbid();

            var comment = await DbContext.Comment.FindAsync(commentID);
            if (comment == null)
                return NotFound();

            if (comment.Author == "admin")
                return Forbid();

            return RedirectToPage("/Blogs/Read", new { id = comment.BlogId });
        }

        public async Task<IActionResult> OnPostDeleteBlogAsync(int blogID)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            var userId = await GetUserId();
            var result = await _blogService.DeleteBlogAsync(userId, blogID);
            if (result.Succeeded)
            {
                return RedirectToPage("/Blogs/Index");
            }

            if (result.Code == Services.Communications.ServiceCode.UnauthorizedAction)
            {
                return Forbid();
            }

            if (result.Code == Services.Communications.ServiceCode.NotFound)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentID)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            var user = await UserManager.GetUserAsync(User);
            var result = await _commentService.DeleteCommentAsync(commentID, user.Id);
            if (result.Succeeded)
            {
                return RedirectToPage("/Blogs/Read", new { id = result.Data });
            }

            if (result.Code == Services.Communications.ServiceCode.UnauthorizedAction)
            {
                return Forbid();
            }

            if (result.Code == Services.Communications.ServiceCode.NotFound)
            {
                return NotFound();
            }

            return Page();
        }
    }
}