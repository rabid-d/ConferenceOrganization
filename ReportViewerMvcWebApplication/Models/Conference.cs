using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReportViewerMvcWebApplication.Models;

public class Conference
{
    [Required, StringLength(128, MinimumLength = 2)]
    public string Name { get; set; }
    [Required, StringLength(128, MinimumLength = 2)]
    public string? Address { get; set; }
    [Required]
    [Weekday]
    [Remote("VerifyConferenceStartDate", "Home")]
    [Display(Name = "Start date")]
    public DateTime DateStart { get; set; } = DateTime.UtcNow;
    [Required]
    [Weekday]
    [Remote("VerifyConferenceEndDate", "Home")]
    [Display(Name = "End date")]
    public DateTime DateEnd { get; set; } = DateTime.UtcNow;
}
