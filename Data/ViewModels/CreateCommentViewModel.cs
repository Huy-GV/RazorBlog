using System;

namespace BlogApp.Data.ViewModel
{
    [Obsolete]
    public class CreateCommentViewModel : EditCommentViewModel
    {
        public int BlogId { get; set; }
    }
}