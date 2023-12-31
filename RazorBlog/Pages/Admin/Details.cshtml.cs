using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using RazorBlog.Data.Validation;
using RazorBlog.Extensions;
using RazorBlog.Models;
using RazorBlog.Services;

namespace RazorBlog.Pages.Admin;

[Authorize(Roles = Roles.AdminRole)]
public class DetailsModel(
    RazorBlogDbContext context,
    UserManager<ApplicationUser> userManager,
    ILogger<DetailsModel> logger,
    IUserModerationService userUserModerationService,
    IPostModerationService postModerationService,
    IPostDeletionScheduler postDeletionService) : RichPageModelBase<DetailsModel>(context, userManager, logger)
{
    private readonly IUserModerationService _userModerationService = userUserModerationService;
    private readonly IPostDeletionScheduler _postDeletionService = postDeletionService;
    private readonly IPostModerationService _postModerationService = postModerationService;

    [BindProperty(SupportsGet =true)] 
    public BanTicket? CurrentBanTicket { get; set; }

    [BindProperty]
    [DateRange(allowsPast: false, allowsFuture: true, ErrorMessage ="Expiry date must be in the future")]
    public DateTime NewBanTicketExpiryDate { get; set; } = DateTime.Now.AddDays(1);

    [BindProperty(SupportsGet = true)]
    [Required]
    public string UserName { get; set; } = string.Empty;

    [BindProperty] 
    public List<HiddenCommentDto> HiddenComments { get; set; } = [];

    [BindProperty] 
    public List<HiddenBlogDto> HiddenBlogs { get; set; } = [];

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
        CurrentBanTicket = await _userModerationService.FindBanTicketByUserNameAsync(userName);

        return Page();
    }

    public async Task<IActionResult> OnPostBanUserAsync()
    {
        if (!ValidatorUtil.TryValidateProperty(NewBanTicketExpiryDate, nameof(NewBanTicketExpiryDate), this))
        {
            return Page();
        }

        if (string.IsNullOrWhiteSpace(UserName) || await UserManager.FindByNameAsync(UserName) == null)
        {
            return BadRequest("User not found");
        }

        return this.NavigateOnResult(
            await _userModerationService.BanUserAsync(UserName, User.Identity?.Name ?? string.Empty, NewBanTicketExpiryDate),
            () => RedirectToPage("Details", new { userName = UserName })
        );
    }

    public async Task<IActionResult> OnPostLiftBanAsync(string userName)
    {
        if (!await _userModerationService.BanTicketExistsAsync(userName))
        {
            return BadRequest();
        }

        return this.NavigateOnResult(
            await _userModerationService.RemoveBanTicketAsync(userName, User.Identity?.Name ?? string.Empty),
            () => RedirectToPage("Details", new { userName })
        );
    }
}