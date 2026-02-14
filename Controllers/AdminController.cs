using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SosyalYardim.Services;
using SosyalYardim.Data;
using Microsoft.EntityFrameworkCore;

namespace SosyalYardim.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ICampaignService _campaignService;
    private readonly IRewardService _rewardService;
    private readonly AppDbContext _context;

    public AdminController(ICampaignService campaignService, IRewardService rewardService, AppDbContext context)
    {
        _campaignService = campaignService;
        _rewardService = rewardService;
        _context = context;
    }

    public IActionResult Index()
    {
        var campaigns = _campaignService.GetAllCampaigns();
        var rewards = _rewardService.GetAllRewards();
        var donations = _context.Donations.ToList();
        var totalDonations = donations.Sum(d => d.Amount);
        
        ViewBag.TotalCampaigns = campaigns.Count;
        ViewBag.ActiveCampaigns = campaigns.Count(c => c.Status == Models.CampaignStatus.Aktif);
        ViewBag.TotalRewards = rewards.Count;
        ViewBag.TotalRaised = campaigns.Sum(c => c.RaisedAmount);
        ViewBag.TotalDonations = donations.Count;
        ViewBag.TotalDonationAmount = totalDonations;

        return View();
    }
}




