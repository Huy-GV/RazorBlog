using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorBlog.Models;

[Table(nameof(Topic))]
public class Topic : IEntity
{
    public int Id { get; set; }

    [StringLength(20)]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreationDate { get; set; }

    public string CreatorUserId { get; set; }

    [DataType("nvarchar(150)")]
    public string Description { get; set; }

    public ApplicationUser Creator { get; set; }
}