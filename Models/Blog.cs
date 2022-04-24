using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Models;

public class Blog : Post, ISoftDeletable
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Introduction { get; set; }
    public uint ViewCount { get; set; } = 0;

    [Required]
    public string CoverImageUri { get; set; }

    public int TopicId { get; set; }
    public Topic Topic { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeleteDate { get; set; } = null;
}