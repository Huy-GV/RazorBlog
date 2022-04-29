namespace RazorBlog.Services.Interfaces
{
    using RazorBlog.Data.ViewModels;
    using RazorBlog.Services.Communications;
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        // signin, sign out, register, deactivate
        Task<Result<Empty, Error>> SignIn(SignInViewModel viewModel);

        Task<Result<Empty, Error>> Register(RegisterViewModel viewModel);

        Task<Result<Empty, Error>> ChangePassword(string password);

        Task<Result<Empty, Error>> DeactivateAccount(string userId);

        Task<bool> UserExists(string? userId);
    }
}