using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Data.ViewModels;

public class LogInViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
}