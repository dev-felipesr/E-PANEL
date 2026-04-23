using E_PANEL.Data;
using E_PANEL.Models;
using E_PANEL.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_PANEL.Controllers;

[Authorize]
public class HomeController(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var etec = user.EtecNumber;
        var userId = user.Id;

        var allPosts = (await db.Posts
            .AsNoTracking()
            .Where(p => p.EtecNumber == etec)
            .Select(p => new PostCardViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                Score = p.Score,
                PostType = p.PostType,
                AuthorName = p.Author != null ? p.Author.FullName : "Usuário",
                CurrentUserVote = p.Votes
                    .Where(v => v.UserId == userId)
                    .Select(v => (int?)v.Value)
                    .FirstOrDefault() ?? 0
            })
            .ToListAsync())
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.CreatedAt)
            .ToList();

        var model = new HomeIndexViewModel
        {
            EtecNumber = etec,
            FeedPosts = allPosts.Where(p => p.PostType == PostType.Regular).ToList(),
            NoticePosts = allPosts.Where(p => p.PostType != PostType.Regular).ToList()
        };

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}
