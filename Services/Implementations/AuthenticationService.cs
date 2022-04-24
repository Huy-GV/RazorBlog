namespace RazorBlog.Services.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using RazorBlog.Data;
    using RazorBlog.Data.Mappers;
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Models;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly RazorBlogDbContext _context;

        public AuthenticationService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AuthenticationService> logger,
            RazorBlogDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public Task<Result> ChangePassword(string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> DeactivateAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return AuthenticationResultMapper.Error(ServiceCode.NotFound, "No user with ID {userId} was found");
            }

            _context.Update(user);
            user.DeleteDate = DateTime.Now;
            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return AuthenticationResultMapper.Success();
        }

        public async Task<Result> Register(RegisterViewModel viewModel)
        {
            string profileUri = "default";
            var user = new ApplicationUser
            {
                UserName = viewModel.UserName,
                EmailConfirmed = true,
                ProfileImageUri = profileUri,
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User created a new account with username {viewModel.UserName}.");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return AuthenticationResultMapper.Success();
            }

            var errors = result.Errors
                .Select(x => new
                {
                    x.Code,
                    x.Description,
                })
                .Aggregate("Errors:\n", (currentError, next) =>
                {
                    return $"{currentError}\n\t{next}";
                });

            return AuthenticationResultMapper.Error(ServiceCode.AuthenticationFailure, errors);
        }

        public async Task<Result> SignIn(SignInViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                    viewModel.UserName,
                    viewModel.Password,
                    viewModel.RememberMe,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return AuthenticationResultMapper.Success();
            }

            if (result.IsLockedOut || result.IsNotAllowed)
            {
                return AuthenticationResultMapper.Error(ServiceCode.AuthenticationFailure, "User is not allowed to sign in.");
            }

            return AuthenticationResultMapper.Error(ServiceCode.InternalError);
        }

        public async Task<bool> UserExists(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return await _userManager.FindByIdAsync(userId) != null;
        }
    }
}