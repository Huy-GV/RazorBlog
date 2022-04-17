namespace RazorBlog.Services.Interfaces
{
    using RazorBlog.Data.ViewModel;
    using RazorBlog.Services.Communications;
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        // signin, sign out, register, deactivate
        Task<Result> SignIn(SignInViewModel viewModel);

        Task<Result> Register(RegisterViewModel viewModel);

        Task<Result> ChangePassword(string password);

        Task<Result> DeactivateAccount(string userId);
    }
}