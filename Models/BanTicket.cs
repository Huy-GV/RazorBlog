using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorBlog.Models;

public class BanTicket
{
    public int Id { get; set; }
    [Required] public string UserName { get; set; }

    [DataType(DataType.DateTime)] public DateTime? Expiry { get; set; }

    public ApplicationUser AppUser { get; set; }
}