using System;

namespace RazorBlog.Data.ViewModel
{
    [Obsolete]
    public class CreateCommentViewModel : EditCommentViewModel
    {
        public int BlogId { get; set; }
    }
}