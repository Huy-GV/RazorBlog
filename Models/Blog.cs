using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Models
{
    public class Blog : Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public int ViewCount { get; set; } = 0;

        // todo: change to image uri
        [Required]
        public string ImagePath { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}