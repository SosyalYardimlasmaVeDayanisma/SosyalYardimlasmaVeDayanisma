using Microsoft.AspNetCore.Mvc;
using AfetPuan.Services;
using AfetPuan.Models;

namespace AfetPuan.Controllers;

public class TransparencyController : Controller
{
    private readonly ITransparencyService _transparencyService;
    private readonly ICampaignService _campaignService;
    private readonly IImpactVerificationService _verificationService;

    public TransparencyController(ITransparencyService transparencyService, ICampaignService campaignService, IImpactVerificationService verificationService)
    {
        _transparencyService = transparencyService;
        _campaignService = campaignService;
        _verificationService = verificationService;
    }

    public IActionResult Index()
    {
        var stats = _transparencyService.GetStats();
        var campaigns = _campaignService.GetAllCampaigns();
        
        // Debug için log
        Console.WriteLine($"[TransparencyController] Toplam kampanya sayısı: {campaigns.Count}");
        if (campaigns.Any())
        {
            Console.WriteLine($"[TransparencyController] İlk kampanya: {campaigns.First().Title} - {campaigns.First().City}");
        }
        
        // JavaScript için uygun formatta kampanya verilerini hazırla
        var campaignsForMap = campaigns.Select(c => new
        {
            id = c.Id,
            title = c.Title,
            city = c.City,
            category = c.Category.ToString(),
            categoryDisplayName = GetCategoryDisplayName(c.Category),
            raisedAmount = c.RaisedAmount,
            goalAmount = c.GoalAmount,
            isVerified = _verificationService.IsVerifiedAsync(c.Id.ToString(), "Campaign").Result
        }).ToList();
        
        Console.WriteLine($"[TransparencyController] Map için hazırlanan veri sayısı: {campaignsForMap.Count}");
        
        ViewBag.CampaignsJson = System.Text.Json.JsonSerializer.Serialize(campaignsForMap);
        ViewBag.VerificationService = _verificationService;
        
        return View(stats);
    }
    
    private string GetCategoryDisplayName(CampaignCategory category)
    {
        return category switch
        {
            CampaignCategory.AfetAcilDurum => "Afet & Acil Durum",
            CampaignCategory.EgitimCocuk => "Eğitim & Çocuk",
            CampaignCategory.Hayvanlar => "Hayvanlar",
            CampaignCategory.CevreVeDoga => "Çevre & Doğa",
            CampaignCategory.GidaTemelIhtiyac => "Gıda & Temel İhtiyaç",
            CampaignCategory.Saglik => "Sağlık",
            _ => category.ToString()
        };
    }
}



