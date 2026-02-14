using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SosyalYardim.Models;
using SosyalYardim.Services;

namespace SosyalYardim.Controllers;

public class HomeController : Controller
{
    private readonly ICampaignService _campaignService;
    private readonly IWalletService _walletService;
    private readonly IRewardService _rewardService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public HomeController(ICampaignService campaignService, IWalletService walletService, IRewardService rewardService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _campaignService = campaignService;
        _walletService = walletService;
        _rewardService = rewardService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        // Öne çıkan kampanyaları getir (max 6)
        var featuredCampaigns = _campaignService.GetFeaturedCampaigns();
        
        // Tüm kategorileri listele
        var allCategories = Enum.GetValues<CampaignCategory>()
            .Select(c => new
            {
                Value = c,
                DisplayName = GetCategoryDisplayName(c)
            })
            .ToList();

        Wallet? wallet = null;
        if (_signInManager.IsSignedIn(User))
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                wallet = _walletService.GetWallet(user.Id);
            }
        }

        var allCampaigns = _campaignService.GetAllCampaigns();
        var activeCampaignCount = allCampaigns.Count(c => c.Status == CampaignStatus.Aktif);
        var totalRaised = allCampaigns.Sum(c => c.RaisedAmount);
        var totalRewards = _rewardService.GetAllRewards().Count;

        ViewBag.Wallet = wallet;
        ViewBag.FeaturedCampaigns = featuredCampaigns;
        ViewBag.AllCategories = allCategories;
        ViewBag.ActiveCampaignCount = activeCampaignCount;
        ViewBag.TotalRaised = totalRaised;
        ViewBag.TotalRewards = totalRewards;

        return View();
    }

    private string GetCategoryDisplayName(CampaignCategory category)
    {
        return category switch
        {
            CampaignCategory.AfetAcilDurum => "Afet & Acil Durum",
            CampaignCategory.EgitimCocuk => "Eğitim & Çocuk",
            CampaignCategory.Saglik => "Sağlık",
            CampaignCategory.Hayvanlar => "Hayvanlar",
            CampaignCategory.GidaTemelIhtiyac => "Gıda & Temel İhtiyaç",
            CampaignCategory.CevreVeDoga => "Çevre & Doğa",
            CampaignCategory.EngelliDogumluBireyler => "Engelli & Doğumlu Bireyler",
            CampaignCategory.YasliDestegi => "Yaşlı Desteği",
            CampaignCategory.KulturSanat => "Kültür & Sanat",
            CampaignCategory.HayalGerceklestirme => "Hayal Gerçekleştirme",
            CampaignCategory.SporVeRekreasyon => "Spor & Rekreasyon",
            CampaignCategory.TeknolojiveYenilikcilik => "Teknoloji & Yenilikçilik",
            _ => "Diğer"
        };
    }    public IActionResult Error()
    {
        return View();
    }
}

