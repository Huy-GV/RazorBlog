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

        public Task<Result<Empty, Error>> ChangePassword(string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result<Empty, Error>> DeactivateAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResultUtil.Failure<Empty>(ServiceCode.NotFound, $"User with ID {userId} was not found.");
                //return AuthenticationResultMapper.Error(ServiceCode.NotFound, "No user with ID {userId} was found");
            }

            _context.Update(user);
            user.DeleteDate = DateTime.Now;
            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return ResultUtil.Success();
        }

        public async Task<Result<Empty, Error>> Register(RegisterViewModel viewModel)
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
                return ResultUtil.Success();
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

            return ResultUtil.Failure(ServiceCode.AuthenticationFailure, errors);
        }

        public async Task<Result<Empty, Error>> SignIn(SignInViewModel viewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                    viewModel.UserName,
                    viewModel.Password,
                    viewModel.RememberMe,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return ResultUtil.Success(new Empty());
            }

            if (result.IsLockedOut || result.IsNotAllowed)
            {
                return ResultUtil.Failure<Empty>(ServiceCode.AuthenticationFailure, "User is not allowed to sign in.");
            }

            return ResultUtil.Failure(ServiceCode.InternalError);
        }

        public async Task<bool> UserExists(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _userManager.FindByIdAsync(userId) != null;
        }
    }
}