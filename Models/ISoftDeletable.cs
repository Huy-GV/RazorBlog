using System;

namespace RazorBlog.Models
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime DeleteDate { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            DeleteDate = DateTime.Now;
        }
    }
}
