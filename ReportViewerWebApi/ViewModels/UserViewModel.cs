using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReportViewerWebApi.ViewModels;

public class UserViewModel
{
    [Required]
    [RegularExpression(@"^(?![.+-])[a-zA-Z0-9.+-]+[^-.+\s][@][a-zA-Z]+[.][a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
    [DefaultValue("admin@gmail.com")]
    public string Email { get; set; }
    [Required, MinLength(4)]
    [DefaultValue("admin")]
    public string Password { get; set; }
}
