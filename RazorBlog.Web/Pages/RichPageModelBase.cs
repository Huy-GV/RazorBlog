﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorBlog.Core.Data;
using RazorBlog.Core.Models;

namespace RazorBlog.Web.Pages;

public class RichPageModelBase<TPageModel> : PageModel where TPageModel : PageModel
{
    public RichPageModelBase(RazorBlogDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<TPageModel> logger)
    {
        DbContext = context;
        UserManager = userManager;
        Logger = logger;
    }

    protected RazorBlogDbContext DbContext { get; }

    protected UserManager<ApplicationUser> UserManager { get; }

    protected ILogger<TPageModel> Logger { get; }

    protected async Task<ApplicationUser?> GetUserOrDefaultAsync()
    {
        return await UserManager.GetUserAsync(User);
    }

    protected bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
}
