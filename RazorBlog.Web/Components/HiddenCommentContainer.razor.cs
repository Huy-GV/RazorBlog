﻿using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RazorBlog.Core.Data;
using RazorBlog.Core.Data.Dtos;
using RazorBlog.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorBlog.Core.Communication;
using RazorBlog.Web.Extensions;

namespace RazorBlog.Web.Components;

public partial class HiddenCommentContainer : RichComponentBase
{
    [Parameter]
    public string UserName { get; set; } = string.Empty;

    [Inject]
    public IDbContextFactory<RazorBlogDbContext> DbContextFactory { get; set; } = null!;

    [Inject]
    public IPostModerationService PostModerationService { get; set; } = null!;

    private IReadOnlyCollection<HiddenCommentDto> HiddenComments { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await LoadHiddenComments();
    }

    private async Task LoadHiddenComments()
    {
        HiddenComments = await GetHiddenComments(UserName);
    }

    private async Task<IReadOnlyCollection<HiddenCommentDto>> GetHiddenComments(string userName)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        return await dbContext.Comment
            .AsNoTracking()
            .Include(c => c.AuthorUser)
            .Where(c => c.AuthorUser.UserName == userName && c.IsHidden)
            .Select(c => new HiddenCommentDto
            {
                Id = c.Id,
                Content = c.Body,
                CreationTime = c.CreationTime,
            })
            .ToListAsync();
    }

    private async Task ForciblyDeleteCommentAsync(int commentId)
    {
        var result = await PostModerationService.ForciblyDeleteCommentAsync(commentId, CurrentUserName);
        if (result != ServiceResultCode.Success)
        {
            this.NavigateOnError(result);
            return;
        }

        await LoadHiddenComments();
    }

    private async Task UnhideCommentAsync(int commentId)
    {
        var result = await PostModerationService.UnhideCommentAsync(commentId, CurrentUserName);
        if (result != ServiceResultCode.Success)
        {
            this.NavigateOnError(result);
            return;
        }

        await LoadHiddenComments();
    }
}
