namespace ReportViewerWebApi.ViewModels.ConferenceEquipment;

public class ConferenceEquipmentViewModel
{
    public string ConferenceName { get; set; }
    public List<BusyEquipmentViewModel> Equipment { get; set; } = new();
}
