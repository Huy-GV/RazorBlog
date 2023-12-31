﻿using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RazorBlog.Data;
using RazorBlog.Data.Dtos;
using RazorBlog.Extensions;
using RazorBlog.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorBlog.Components;

public partial class HiddenBlogContainer : RichComponentBase
{
    [Parameter]
    public string UserName { get; set; } = string.Empty;

    [Inject]
    public IDbContextFactory<RazorBlogDbContext> DbContextFactory { get; set; } = null!;

    [Inject]
    public IPostModerationService PostModerationService { get; set; } = null!;

    public IReadOnlyCollection<HiddenBlogDto> HiddenBlogs { get; private set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await LoadHiddenBlogs();
    }

    private async Task<List<HiddenBlogDto>> GetHiddenBlogs(string username)
    {
        using var dbContext = await DbContextFactory.CreateDbContextAsync();

        return await dbContext.Blog
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

    private async Task LoadHiddenBlogs()
    {
        HiddenBlogs = await GetHiddenBlogs(UserName);
    }

    private async Task ForciblyDeleteBlogAsync(int blogId)
    {
        var result = await PostModerationService.ForciblyDeleteBlogAsync(blogId, CurrentUserName);
        this.NavigateOnError(result);

        await LoadHiddenBlogs();
    }

    private async Task UnhideBlogAsync(int blogId)
    {
        var result = await PostModerationService.UnhideBlogAsync(blogId, CurrentUserName);
        this.NavigateOnError(result);

        await LoadHiddenBlogs();
    }
}