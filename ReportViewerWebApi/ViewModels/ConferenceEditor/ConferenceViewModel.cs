using System.ComponentModel.DataAnnotations;

namespace ReportViewerWebApi.ViewModels.ConferenceEditor;

public class ConferenceViewModel
{
    [Required, StringLength(128, MinimumLength = 2)]
    public string Name { get; set; }
    [Required, StringLength(128, MinimumLength = 2)]
    public string? Address { get; set; }
    [Required]
    public DateTime DateStart { get; set; }
    [Required]
    [Weekday]
    public DateTime DateEnd { get; set; }
}
