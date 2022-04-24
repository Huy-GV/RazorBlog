using Microsoft.AspNetCore.Http;
using RazorBlog.Data.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Data.ViewModels
{
    [Obsolete]
    public class EditBlogViewModel : CreateBlogViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Change cover image")]
        [FileType(".jpg", ".jpeg", ".png", ErrorMessage = "Only jpg/jpeg and png files are allowed")]
        public new IFormFile CoverImage { get; set; }
    }
}