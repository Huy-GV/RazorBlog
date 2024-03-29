﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorBlog.Core.Data;
using RazorBlog.Core.Data.Dtos;
using RazorBlog.Core.Models;
using RazorBlog.Core.Services;

namespace RazorBlog.Web.Pages.Blogs;

[AllowAnonymous]
public class IndexModel : RichPageModelBase<IndexModel>
{
    private readonly IBlogReader _blogReader;
    public IndexModel(
        RazorBlogDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<IndexModel> logger,
        IBlogReader blogReader) : base(context, userManager, logger)
    {
        _blogReader = blogReader;
    }

    [BindProperty]
    public IReadOnlyCollection<IndexBlogDto> Blogs { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string SearchString { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        Blogs = await _blogReader.GetBlogsAsync(searchString: SearchString.Trim().Trim(' '));
    }
}
