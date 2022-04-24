using Microsoft.AspNetCore.Http;
using RazorBlog.Data.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Data.ViewModels
{
    [Obsolete]
    public class CreateBlogViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 200)]
        public string Content { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Description { get; set; }

        [Display(Name = "Cover image")]
        [Required]
        [FileType(".jpg", ".jpeg", ".png", ErrorMessage = "Only jpg/jpeg and png files are allowed")]
        public IFormFile CoverImage { get; set; }
    }
}