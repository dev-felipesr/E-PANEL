using System.ComponentModel.DataAnnotations;

namespace E_PANEL.Models;

public class ComplaintVote
{
    public int Id { get; set; }

    [Range(-1, 1)]
    public int Value { get; set; }

    public int ComplaintId { get; set; }
    public Complaint? Complaint { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
}
