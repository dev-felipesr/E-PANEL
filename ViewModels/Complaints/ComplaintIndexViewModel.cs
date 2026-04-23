namespace E_PANEL.ViewModels.Complaints;

public class ComplaintIndexViewModel
{
    public string EtecNumber { get; set; } = string.Empty;
    public List<ComplaintCardViewModel> Complaints { get; set; } = [];
}
