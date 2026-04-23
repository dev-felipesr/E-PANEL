using E_PANEL.Data;
using E_PANEL.Models;
using E_PANEL.Services;
using E_PANEL.ViewModels.Complaints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_PANEL.Controllers;

[Authorize]
public class ComplaintsController(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    IProfanityFilter profanityFilter) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var complaints = (await db.Complaints
            .AsNoTracking()
            .Where(c => c.EtecNumber == user.EtecNumber)
            .Select(c => new ComplaintCardViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                Score = c.Score,
                HasProfanity = c.HasProfanity,
                AuthorName = c.IsAnonymous
                    ? "Anônimo"
                    : c.Author != null
                        ? c.Author.FullName
                        : "Usuário",
                CurrentUserVote = c.Votes
                    .Where(v => v.UserId == user.Id)
                    .Select(v => (int?)v.Value)
                    .FirstOrDefault() ?? 0
            })
            .ToListAsync())
            .OrderByDescending(c => c.Score)
            .ThenByDescending(c => c.CreatedAt)
            .ToList();

        return View(new ComplaintIndexViewModel
        {
            EtecNumber = user.EtecNumber,
            Complaints = complaints
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateComplaintViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateComplaintViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var hasProfanity = profanityFilter.ContainsProfanity(model.Title) || profanityFilter.ContainsProfanity(model.Content);
        var sanitizedTitle = profanityFilter.Sanitize(model.Title.Trim());
        var sanitizedContent = profanityFilter.Sanitize(model.Content.Trim());

        db.Complaints.Add(new Complaint
        {
            Title = sanitizedTitle,
            Content = sanitizedContent,
            IsAnonymous = model.IsAnonymous,
            HasProfanity = hasProfanity,
            EtecNumber = user.EtecNumber,
            AuthorId = model.IsAnonymous ? null : user.Id
        });

        await db.SaveChangesAsync();

        if (hasProfanity)
        {
            TempData["Warning"] = "Palavras impróprias foram censuradas automaticamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Vote([FromBody] VoteRequest request)
    {
        if (request.Value is not (1 or -1))
        {
            return BadRequest("Voto inválido.");
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var complaint = await db.Complaints
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.EtecNumber == user.EtecNumber);

        if (complaint is null)
        {
            return NotFound();
        }

        var vote = await db.ComplaintVotes
            .FirstOrDefaultAsync(v => v.ComplaintId == complaint.Id && v.UserId == user.Id);

        var userVote = request.Value;
        if (vote is null)
        {
            db.ComplaintVotes.Add(new ComplaintVote
            {
                ComplaintId = complaint.Id,
                UserId = user.Id,
                Value = request.Value
            });
            complaint.Score += request.Value;
        }
        else if (vote.Value == request.Value)
        {
            complaint.Score -= vote.Value;
            db.ComplaintVotes.Remove(vote);
            userVote = 0;
        }
        else
        {
            complaint.Score += request.Value * 2;
            vote.Value = request.Value;
        }

        await db.SaveChangesAsync();
        return Json(new { score = complaint.Score, userVote });
    }

    public sealed class VoteRequest
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
