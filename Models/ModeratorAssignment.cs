using System;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Models;

public class ModeratorAssignment : IEntity
{
    public string ModeratorId { get; set; }
    public int CommunityId { get; set; }

    [DataType("date")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    public uint BanTicketCount { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
    public DateTime DeleteDate { get; set; } = DateTime.Now;
    public ApplicationUser Moderator { get; set; }
    public Topic Community { get; set; }
}