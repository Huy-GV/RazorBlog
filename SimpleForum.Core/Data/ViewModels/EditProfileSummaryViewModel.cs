using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Core.Data.ViewModels;

public class EditProfileSummaryViewModel
{
    [StringLength(200, MinimumLength = 10)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Summary { get; set; } = string.Empty;
}
