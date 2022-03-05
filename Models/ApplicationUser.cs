using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class ApplicationUser : IdentityUser, IEntity, ISoftDeletable
    {
        [DataType(DataType.Date)]
        public DateTime? RegistrationDate { get; set; }
        public int BanCount { get; set; } = 0;
        [Required]
        public string Country { get; set; }
        public string Description { get; set; }
        public string ProfilePicturePath { get; set; } = "default.jpg";
        public bool IsDeleted { get; set; } = false;
        public DateTime DeleteDate { get; set; } = DateTime.Now;
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
        #region Deleted properties
        [NotMapped]
        public override bool LockoutEnabled { get; set; }
        [NotMapped]
        public override int AccessFailedCount { get; set; }
        [NotMapped]
        public override string? PhoneNumber { get; set; }
        [NotMapped]
        public override string? SecurityStamp { get; set; }
        [NotMapped]
        public override DateTimeOffset? LockoutEnd { get; set; }
        [NotMapped]
        public override bool TwoFactorEnabled { get; set; }
        [NotMapped]
        public override bool PhoneNumberConfirmed { get; set; }
        #endregion
    }
}
