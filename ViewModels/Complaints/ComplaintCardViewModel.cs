namespace E_PANEL.ViewModels.Complaints;

public class ComplaintCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public int Score { get; set; }
    public int CurrentUserVote { get; set; }
    public bool HasProfanity { get; set; }
}
