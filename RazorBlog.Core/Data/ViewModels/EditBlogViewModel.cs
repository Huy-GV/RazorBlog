using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RazorBlog.Core.Data.Validation;

namespace RazorBlog.Core.Data.ViewModels;

public class EditBlogViewModel : BlogViewModel
{
    [Required]
    public int Id { get; set; }

    [Display(Name = "Change cover image")]
    [FileType("jpg", "jpeg", "png", ErrorMessage = "File type must be one of: .png, .jpeg, .jpg")]
    public IFormFile? CoverImage { get; set; }
}