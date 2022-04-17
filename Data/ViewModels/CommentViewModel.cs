namespace RazorBlog.Data.ViewModels
{
    public class CommentViewModel : PostViewModel
    {
        public int BlogId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}