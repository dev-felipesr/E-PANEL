using E_PANEL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_PANEL.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostVote> PostVotes => Set<PostVote>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<ComplaintVote> ComplaintVotes => Set<ComplaintVote>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PostVote>()
            .HasIndex(v => new { v.PostId, v.UserId })
            .IsUnique();

        builder.Entity<ComplaintVote>()
            .HasIndex(v => new { v.ComplaintId, v.UserId })
            .IsUnique();

        builder.Entity<Post>()
            .HasIndex(p => new { p.EtecNumber, p.Score, p.CreatedAt });

        builder.Entity<Complaint>()
            .HasIndex(c => new { c.EtecNumber, c.Score, c.CreatedAt });

        builder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Complaint>()
            .HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Post>()
            .Property(p => p.Score)
            .HasDefaultValue(0);

        builder.Entity<Complaint>()
            .Property(c => c.Score)
            .HasDefaultValue(0);
    }
}
