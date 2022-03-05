using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Post : IEntity
    {
        [Required, MaxLength(2500)]
        public virtual string Content { get; set; }
        [MaxLength(255)]
        [DataType("date"), Required]
        public DateTime Date { get; set; }
        public string Author => AppUser.UserName;
        public string AppUserID { get; set; }
        public ApplicationUser AppUser { get; set; }
        public bool IsHidden { get; set; }
    }
}
