﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Post
    {
        [Required, MaxLength(2500)]
        public virtual string Content { get; set; }

        //TODO: rename to something else
        [MaxLength(255)]
        public string SuspensionExplanation { get; set; } = string.Empty;

        [MaxLength(255)]
        [DataType(DataType.Date), Required]
        public DateTime Date { get; set; }

        [Obsolete]
        public string Author { get; set; }

        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
        public bool IsHidden => SuspensionExplanation != string.Empty;
    }
}