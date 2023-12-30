﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorBlog.Data.ViewModels;
using RazorBlog.Models;
using RazorBlog.Services;

namespace RazorBlog.Pages.Authentication;

[AllowAnonymous]
public class RegisterModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<RegisterModel> logger,
    IImageStorage imageStorage) : PageModel
{
    private readonly IImageStorage _imageStorage = imageStorage;
    private readonly ILogger<RegisterModel> _logger = logger;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [BindProperty]
    public CreateUserViewModel CreateUserViewModel { get; set; } = null!;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new ApplicationUser
        {
            UserName = CreateUserViewModel.UserName,
            EmailConfirmed = true,
            RegistrationDate = DateTime.Now,
            ProfileImageUri = CreateUserViewModel.ProfilePicture == null
                ? GetDefaultProfileImageUri()
                : await UploadProfileImage(CreateUserViewModel.ProfilePicture)
        };

        var result = await _userManager.CreateAsync(user, CreateUserViewModel.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            
            return Page();
        }

        _logger.LogInformation($"User created a new account with username {CreateUserViewModel.UserName}.");
        await _signInManager.SignInAsync(user, false);

        return LocalRedirect(Url.Content("~/"));
    }

    private async Task<string> UploadProfileImage(IFormFile image)
    {
        try
        {
            return await _imageStorage.UploadProfileImageAsync(image);
        }
        catch (Exception ex)
        {
            // todo: un-hardcode the default image path
            _logger.LogError($"Failed to upload new profile picture: {ex}");
            return GetDefaultProfileImageUri();
        }
    }

    private static string GetDefaultProfileImageUri()
    {
        return Path.Combine("ProfileImage", "default.jpg");
    }
}