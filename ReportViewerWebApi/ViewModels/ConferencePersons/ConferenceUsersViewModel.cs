namespace ReportViewerWebApi.ViewModels.ConferencePersons;

public class ConferenceUsersViewModel
{
    public string ConferenceName { get; set; }
    public List<ConferencePersonViewModel> Chairpersons { get; set; } = new();
    public List<ConferencePersonViewModel> Speakers { get; set; } = new();
}
