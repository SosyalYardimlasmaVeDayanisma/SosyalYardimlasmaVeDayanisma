using Microsoft.AspNetCore.Mvc;
using SosyalYardim.Services;
using SosyalYardim.Models;

namespace SosyalYardim.Controllers;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class CampaignsController : Controller
{
    private readonly ICampaignService _campaignService;
    private readonly IImpactVerificationService _verificationService;

    public CampaignsController(ICampaignService campaignService, IImpactVerificationService verificationService)
    {
        _campaignService = campaignService;
        _verificationService = verificationService;
    }

    public IActionResult Index(string? search, string? city, DisasterType? disasterType, CampaignStatus? status, CampaignCategory? category)
    {
        var campaigns = _campaignService.FilterCampaigns(search, city, disasterType, status, category);
        
        // Zaman bağlamlı kampanyaları önce göster
        campaigns = campaigns
            .OrderByDescending(c => c.IsTimeSensitive)
            .ThenByDescending(c => c.CreatedAt)
            .ToList();
        
        ViewBag.Search = search;
        ViewBag.City = city;
        ViewBag.DisasterType = disasterType;
        ViewBag.Status = status;
        ViewBag.Category = category;
        ViewBag.Cities = new[] { "İstanbul", "Ankara", "İzmir", "Kahramanmaraş", "Antalya", "Muğla", "Bursa" };
        ViewBag.DisasterTypes = Enum.GetValues<DisasterType>();
        ViewBag.Statuses = Enum.GetValues<CampaignStatus>();
        ViewBag.Categories = Enum.GetValues<CampaignCategory>();

        return View(campaigns);
    }

    public async Task<IActionResult> Details(string id)
    {
        var campaign = _campaignService.GetCampaignById(id);
        
        if (campaign == null)
        {
            return NotFound();
        }

        var verification = await _verificationService.GetVerificationAsync(id, "Campaign");
        ViewBag.Verification = verification;

        return View(campaign);
    }
}



