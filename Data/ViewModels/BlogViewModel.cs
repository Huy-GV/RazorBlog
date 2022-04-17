using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Data.ViewModels
{
    public class BlogViewModel : PostViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 200)]
        public override string Content { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Description { get; set; }

        public int TopicId { get; set; }

        [Required]
        public IFormFile CoverImage { get; set; }
    }
}