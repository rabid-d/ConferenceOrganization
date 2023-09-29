namespace ReportViewerWebApi.ViewModels.ConferenceSchedule;

public class ConferenceScheduleViewModel
{
    public string ConferenceName { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }

    public List<SectionScheduleViewModel> Sections { get; set; } = new();
}
