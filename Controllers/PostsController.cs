using E_PANEL.Data;
using E_PANEL.Infrastructure;
using E_PANEL.Models;
using E_PANEL.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_PANEL.Controllers;

[Authorize]
public class PostsController(
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePostViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var isProfessor = await userManager.IsInRoleAsync(user, AppRoles.Professor);
        if (!isProfessor && model.PostType != PostType.Regular)
        {
            ModelState.AddModelError(string.Empty, "Apenas professores podem publicar avisos e eventos.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        db.Posts.Add(new Post
        {
            Title = model.Title.Trim(),
            Content = model.Content.Trim(),
            PostType = model.PostType,
            EtecNumber = user.EtecNumber,
            AuthorId = user.Id
        });

        await db.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
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

        var post = await db.Posts
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.EtecNumber == user.EtecNumber);

        if (post is null)
        {
            return NotFound();
        }

        if (post.PostType is not PostType.Regular)
        {
            return BadRequest("Avisos e eventos não aceitam votos.");
        }

        var vote = await db.PostVotes
            .FirstOrDefaultAsync(v => v.PostId == post.Id && v.UserId == user.Id);

        var userVote = request.Value;
        if (vote is null)
        {
            db.PostVotes.Add(new PostVote
            {
                PostId = post.Id,
                UserId = user.Id,
                Value = request.Value
            });
            post.Score += request.Value;
        }
        else if (vote.Value == request.Value)
        {
            post.Score -= vote.Value;
            db.PostVotes.Remove(vote);
            userVote = 0;
        }
        else
        {
            post.Score += request.Value * 2;
            vote.Value = request.Value;
        }

        await db.SaveChangesAsync();
        return Json(new { score = post.Score, userVote });
    }

    public sealed class VoteRequest
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
