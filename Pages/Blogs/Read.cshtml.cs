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
using System;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Blogs
{
    [AllowAnonymous]
    public class ReadModel : BasePageModel<ReadModel>
    {
        [BindProperty]
        public CreateCommentViewModel CreateCommentViewModel { get; set; }

        [BindProperty]
        public EditCommentViewModel EditCommentViewModel { get; set; }

        public Blog Blog { get; set; }
        public DetailedBlogDto DetailedBlogDto { get; set; }

        public ReadModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ReadModel> logger) : base(
                context, userManager, logger)
        {
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await DbContext.Blog
                .Include(blog => blog.AppUser)
                .Include(blog => blog.Comments)
                    .ThenInclude(comment => comment.AppUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(blog => blog.Id == id);

            if (Blog == null)
            {
                return NotFound();
            }

            await IncrementViewCountAsync(id.Value);
            ViewData["IsSuspended"] = false;

            DetailedBlogDto = DetailedBlogDto.From(Blog);

            return Page();
        }

        // todo: track this.
        private async Task IncrementViewCountAsync(int id)
        {
            //get blog again to avoid tracking errors
            var blog = await DbContext.Blog.FindAsync(id);
            blog.ViewCount++;
            DbContext.Blog.Update(blog);
            await DbContext.SaveChangesAsync();
        }

        public async Task<IActionResult> OnPostCreateCommentAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            if (!ModelState.IsValid)
            {
                Logger.LogError("Model state invalid when submitting comments");
                //TODO: return an appropriate response
                return NotFound();
            }

            var user = await UserManager.GetUserAsync(User);
            var username = user.UserName;

            // var comment = new Comment
            // {
            //     Author = user.UserName,
            //     Content = CreateCommentVM.Content,
            //     Date = DateTime.Now,
            //     BlogID = CreateCommentVM.BlogID
            // };

            var entry = DbContext.Comment.Add(new Comment
            {
                Date = DateTime.Now,
                AppUserId = user.Id
            });

            entry.CurrentValues.SetValues(CreateCommentViewModel);
            await DbContext.SaveChangesAsync();

            return RedirectToPage("/Blogs/Read", new { id = CreateCommentViewModel.BlogId });
        }

        public async Task<IActionResult> OnPostEditCommentAsync(int commentID)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            if (!ModelState.IsValid)
            {
                Logger.LogError("Model state invalid when editting comments");
                return NotFound();
            }

            var user = await UserManager.GetUserAsync(User);
            var comment = await DbContext.Comment.FindAsync(commentID);
            if (user.UserName != comment.Author)
                return Forbid();

            comment.Content = EditCommentViewModel.Content;
            await DbContext.SaveChangesAsync();

            return RedirectToPage("/Blogs/Read", new { id = comment.BlogId });
        }

        public async Task<IActionResult> OnPostHideBlogAsync(int blogID)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

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

        public async Task<IActionResult> OnPostHideCommentAsync(int commentID)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

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
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            var blog = await DbContext.Blog.FindAsync(blogID);

            if (User.Identity.Name != blog.Author && !User.IsInRole(Roles.AdminRole))
                return Forbid();

            DbContext.Blog.Remove(blog);
            await DbContext.SaveChangesAsync();

            return RedirectToPage("/Blogs/Index");
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentID)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            var comment = await DbContext.Comment.FindAsync(commentID);

            if (User.Identity.Name != comment.Author && !User.IsInRole(Roles.AdminRole))
                return Forbid();

            DbContext.Comment.Remove(comment);
            await DbContext.SaveChangesAsync();

            return RedirectToPage("/Blogs/Read", new { id = comment.BlogId });
        }
    }
}