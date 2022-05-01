using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Data.ViewModels
{
    public class CommentViewModel
    {
        public int BlogId { get; set; }

        [Required]
        [StringLength(100)]
        public string CommentContent { get; set; }

        // public int? ParentCommentId { get; set; }
    }
}