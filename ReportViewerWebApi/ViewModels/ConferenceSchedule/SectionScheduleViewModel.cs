namespace ReportViewerWebApi.ViewModels.ConferenceSchedule;

public class SectionScheduleViewModel
{
    public string Name { get; set; }
    public string Chairperson { get; set; }
    public string Room { get; set; }
    public List<TalkScheduleViewModel> Talks { get; set; } = new();
}
