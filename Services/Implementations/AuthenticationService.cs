namespace BlogApp.Services.Implementations
{
    using BlogApp.Data.ViewModel;
    using BlogApp.Models;
    using BlogApp.Services.Communications;
    using BlogApp.Services.Interfaces;
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