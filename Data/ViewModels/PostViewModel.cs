using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.ViewModels
{
    public class PostViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public virtual string Content { get; set; }
    }
}