using System.ComponentModel.DataAnnotations;

namespace RazorBlog.Data.ViewModel
{
    public class PersonalDetailsViewModel
    {
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Country { get; set; }

        [Required]
        [StringLength(350, MinimumLength = 20)]
        public string Description { get; set; }
    }
}