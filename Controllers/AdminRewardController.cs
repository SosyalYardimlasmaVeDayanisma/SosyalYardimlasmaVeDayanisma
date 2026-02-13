using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AfetPuan.Services;
using AfetPuan.Models;
using AfetPuan.Data;

namespace AfetPuan.Controllers;

[Authorize(Roles = "Admin")]
public class AdminRewardController : Controller
{
    private readonly IRewardService _rewardService;
    private readonly AppDbContext _context;

    public AdminRewardController(IRewardService rewardService, AppDbContext context)
    {
        _rewardService = rewardService;
        _context = context;
    }

    public IActionResult Index()
    {
        var rewards = _rewardService.GetAllRewards();
        return View(rewards);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Reward reward)
    {
        if (string.IsNullOrEmpty(reward.Title) || reward.CostPoints <= 0)
        {
            ViewBag.Error = "Ödül adı ve puan gereklidir";
            return View();
        }

        reward.Id = $"reward_{Guid.NewGuid()}";

        _context.Rewards.Add(reward);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Edit(string id)
    {
        var reward = _rewardService.GetRewardById(id);
        if (reward == null)
            return NotFound();

        return View(reward);
    }

    [HttpPost]
    public IActionResult Edit(string id, Reward reward)
    {
        var existing = _context.Rewards.FirstOrDefault(r => r.Id == id);
        if (existing == null)
            return NotFound();

        existing.Title = reward.Title;
        existing.Description = reward.Description;
        existing.MerchantName = reward.MerchantName;
        existing.City = reward.City;
        existing.Category = reward.Category;
        existing.CostPoints = reward.CostPoints;
        existing.Stock = reward.Stock;
        existing.ValidUntil = reward.ValidUntil;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        var reward = _context.Rewards.FirstOrDefault(r => r.Id == id);
        if (reward == null)
            return NotFound();

        _context.Rewards.Remove(reward);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
