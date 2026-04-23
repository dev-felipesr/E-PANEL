using System.ComponentModel.DataAnnotations;

namespace E_PANEL.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2500)]
    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [MaxLength(20)]
    public string EtecNumber { get; set; } = string.Empty;

    public int Score { get; set; }

    public PostType PostType { get; set; } = PostType.Regular;

    [Required]
    public string AuthorId { get; set; } = string.Empty;
    public ApplicationUser? Author { get; set; }

    public ICollection<PostVote> Votes { get; set; } = new List<PostVote>();
}
