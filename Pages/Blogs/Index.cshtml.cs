using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorBlog.Data;
using RazorBlog.Data.DTOs;
using RazorBlog.Models;
using RazorBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorBlog.Pages.Blogs
{
    [AllowAnonymous]
    public class IndexModel : BasePageModel<IndexModel>
    {
        private readonly IBlogService _blogService;
        public IList<BlogDto> Blogs { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; } = null;

        public IndexModel(
            RazorBlogDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger,
            IBlogService blogService) : base(
                context, userManager, logger)
        {
            _blogService = blogService;
        }

        public async Task OnGetAsync()
        {
            Blogs = await _blogService.GetAllBlogsAsync(SearchString);
        }
    }
}