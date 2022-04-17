using System;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Models;

public class BanTicket : IEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }

    [DataType("nvarchar(200)")]
    public string Comment { get; set; }

    public string ModeratorId { get; set; }
    public int TopicId { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public ApplicationUser AppUser { get; set; }
    public ApplicationUser Moderator { get; set; }
}