using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CommunityModeration : IEntity, ISoftDeletable
    {
        public string ModeratorId { get; set; } 
        public int CommunityId { get; set; }
        [DataType("date")]
        public DateTime StartDate { get; set; } = DateTime.Now;
        public uint BanTicketIssued { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public DateTime DeleteDate { get; set; } = DateTime.Now;
        public ApplicationUser Moderator { get; set; }
        public Community Community { get; set; }

    }
}
