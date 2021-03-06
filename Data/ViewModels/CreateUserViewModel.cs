using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RazorBlog.Data.ViewModels;

public class CreateUserViewModel
{
    [Required]
    [Display(Name = "UserName")]
    public string UserName { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "Profile picture")]
    public IFormFile ProfilePicture { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3)]
    [Display(Name = "Country")]
    public string Country { get; set; }
}