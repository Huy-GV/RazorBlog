using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class CommunitySubscription : IEntity
    {
        public string UserId { get; set; }
        public int CommunityId { get; set; }
        [DataType("date")]
        public DateTime SubscriptionDate { get; set; } = DateTime.Now;
        public ApplicationUser AppUser { get; set; }
        public Community Community { get; set; }
    }
}
