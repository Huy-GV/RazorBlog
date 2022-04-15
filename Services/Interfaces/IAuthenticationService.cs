namespace BlogApp.Services.Interfaces
{
    using BlogApp.Data.ViewModel;
    using BlogApp.Services.Communications;
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        // signin, sign out, register, deactivate
        Task<Result> SignIn(SignInViewModel viewModel);

        Task<Result> SignOut();

        Task<Result> Register(RegisterViewModel viewModel);

        Task<Result> ChangePassword(string password);

        Task<Result> DeactivateAccount(string userId);
    }
}