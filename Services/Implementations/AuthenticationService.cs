namespace RazorBlog.Services.Implementations
{
    using RazorBlog.Data.ViewModel;
    using RazorBlog.Models;
    using RazorBlog.Services.Communications;
    using RazorBlog.Services.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public Task<Result> ChangePassword(string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> DeactivateAccount(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> Register(RegisterViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> SignIn(SignInViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> SignOut()
        {
            throw new System.NotImplementedException();
        }
    }
}