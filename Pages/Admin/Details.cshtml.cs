using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogApp.Pages;
using BlogApp.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BlogApp.Data.DTOs;
using BlogApp.Models;

namespace BlogApp.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class DetailsModel : PageModel
    {
        private ApplicationDbContext _context { get; }
        private IAuthorizationService _authorizationService { get; }
        private UserManager<IdentityUser> _userManager { get; }
        private readonly ILogger<AdminModel> _logger;
        public DetailsModel(ApplicationDbContext context,
                          IAuthorizationService authorizationService,
                          UserManager<IdentityUser> userManager,
                          ILogger<AdminModel> logger)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public Suspension SuspensionTicket { get; set; }
        public async Task<IActionResult> OnGetAsync(string? username)
        {
            if (username == null)
                return NotFound();
            _logger.LogInformation($"username from onget is : {username}");
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _logger.LogInformation("User not found");
                return NotFound();
            }
            
            ViewData["UserDTO"] = GetUserDTO(username);
            ViewData["SuspendedBlogs"] = GetSuspendedBlogs(username);
            ViewData["SuspendedComments"] = GetSuspendedComments(username);
            ViewData["Suspension"] = GetSuspension(username);

            _logger.LogInformation($"suspension: {GetSuspension(username)}");
            return Page();
        }
        private Suspension GetSuspension(string username) {
            return _context.Suspension.FirstOrDefault(s => s.Username == username);
        }
        private UserDTO GetUserDTO(string username) {
            return new UserDTO()
            {
                Username = username,
                BlogCount = _context.Blog
                    .Where(blog => blog.Author == username)
                    .ToList()
                    .Count,
                CommentCount = _context.Comment
                    .Where(comment => comment.Author == username)
                    .ToList()
                    .Count
            };
        }
        private List<Blog> GetSuspendedBlogs(string username) {
            return _context.Blog
                .Where(blog => blog.Author == username)
                .Where(blog => blog.IsHidden)
                .ToList();
        }
        private List<Comment> GetSuspendedComments(string username) {
            return _context.Comment
                .Where(comment => comment.Author == username)
                .Where(comment => comment.IsHidden)
                .ToList();
        }

        public async Task<IActionResult> OnPostSuspendUserAsync() 
        {
            if (!SuspensionExists(SuspensionTicket.Username)) {
                _context.Suspension.Add(SuspensionTicket);
                await _context.SaveChangesAsync();
            } else {
                _logger.LogInformation("User has already been suspended");
            }
            return RedirectToPage("Details", new { username = SuspensionTicket.Username });
        }
        public async Task<IActionResult> OnPostLiftSuspensionAsync(string username) 
        {
            if (SuspensionExists(username)) {
                var suspension = _context.Suspension.FirstOrDefault(s => s.Username == username);
                _context.Suspension.Remove(suspension);
                await _context.SaveChangesAsync();
            } else {
                _logger.LogInformation("User has no suspensions");
            }
            _logger.LogInformation("username lifted " + username);
            return RedirectToPage("Details", new { username });
        }
        private bool SuspensionExists(string username) {
            return _context.Suspension.Any(s => s.Username == username);
        }
    }
}