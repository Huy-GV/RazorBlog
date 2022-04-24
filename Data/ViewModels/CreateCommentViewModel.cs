using System;

namespace RazorBlog.Data.ViewModels
{
    [Obsolete]
    public class CreateCommentViewModel : EditCommentViewModel
    {
        public int BlogId { get; set; }
    }
}