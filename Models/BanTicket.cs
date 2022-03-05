using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BanTicket : IEntity, ISoftDeletable
    { 
        public int Id { get; set; } 
        public string UserId { get; set; } 
        [DataType("nvarchar(200)")]
        public string Comment { get; set; }
        public string ModeratorId { get; set; }
        public int CommunityId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DeleteDate { get; set; } = DateTime.Now;
        public ApplicationUser AppUser { get; set; }
        public ApplicationUser Moderator { get; set; }
    }
}
