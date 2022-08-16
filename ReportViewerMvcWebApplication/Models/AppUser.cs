using System.ComponentModel.DataAnnotations;

namespace ReportViewerMvcWebApplication.Models;

public class AppUser
{
    [Required]
    [RegularExpression(@"^(?![.+-])[a-zA-Z0-9.+-]+[^-.+\s][@][a-zA-Z]+[.][a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
    public string Email { get; set; }
    [Required, MinLength(4)]
    public string Password { get; set; }
}
