using System;

namespace RazorBlog.Models;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeleteDate { get; set; }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeleteDate = DateTime.Now;
    }

    public void Recover()
    {
        IsDeleted = false;
        DeleteDate = null;
    }
}