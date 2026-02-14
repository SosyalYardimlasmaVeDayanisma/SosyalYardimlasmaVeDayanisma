using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SosyalYardim.Services;
using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Controllers;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class RewardsController : Controller
{
    private readonly IRewardService _rewardService;
    private readonly IWalletService _walletService;
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly INotificationService _notificationService;

    public RewardsController(IRewardService rewardService, IWalletService walletService, UserManager<User> userManager, AppDbContext context, INotificationService notificationService)
    {
        _rewardService = rewardService;
        _walletService = walletService;
        _userManager = userManager;
        _context = context;
        _notificationService = notificationService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, RewardCategory? category, ContributionType? tab)
    {
        var user = await _userManager.GetUserAsync(User);
        
        List<Reward> rewards;
        if (!string.IsNullOrEmpty(search))
        {
            rewards = _rewardService.SearchRewards(search);
        }
        else if (tab.HasValue)
        {
            rewards = _rewardService.GetRewardsByContributionType(tab.Value);
        }
        else
        {
            rewards = _rewardService.GetRewardsByCategory(category);
        }
        
        var wallet = user != null ? _walletService.GetWallet(user.Id) : null;

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Tab = tab ?? ContributionType.BaskasıIcinDonustur; // Varsayılan: Başkası İçin
        ViewBag.Wallet = wallet;
        ViewBag.IsLoggedIn = user != null;
        ViewBag.Categories = Enum.GetValues<RewardCategory>();

        return View(rewards);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(string id)
    {
        var user = await _userManager.GetUserAsync(User);
        var reward = _rewardService.GetRewardById(id);
        
        if (reward == null)
        {
            return NotFound();
        }

        var wallet = user != null ? _walletService.GetWallet(user.Id) : null;
        ViewBag.Wallet = wallet;
        ViewBag.IsLoggedIn = user != null;

        return View(reward);
    }

    [Authorize]
    [HttpPost]
    [Route("/Rewards/Redeem")]
    public async Task<IActionResult> Redeem([FromForm] string id)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı" });
            }

            var reward = _rewardService.GetRewardById(id);
            if (reward == null)
            {
                return Json(new { success = false, message = "Ödül bulunamadı" });
            }
            
            if (!reward.IsValid)
            {
                return Json(new { success = false, message = "Ödülün süresi dolmuş" });
            }
            
            if (!reward.InStock)
            {
                return Json(new { success = false, message = "Ödül stokta kalmamış" });
            }

            var wallet = _walletService.GetWallet(user.Id);
            if (wallet.Balance < reward.CostPoints)
            {
                return Json(new { success = false, message = $"Yetersiz bakiye. Gereken: {reward.CostPoints}, Mevcut: {wallet.Balance}" });
            }

            var (success, code, qrCode) = _rewardService.RedeemReward(id);
            
            if (success)
            {
                var transaction = new WalletTransaction
                {
                    Id = $"tx_{Guid.NewGuid()}",
                    Type = TransactionType.Spend,
                    Amount = -reward.CostPoints,
                    Ref = $"Ödül #{id}",
                    CreatedAt = DateTime.Now,
                    Description = reward.Title
                };

                _walletService.AddTransaction(user.Id, transaction);

                // Bildirim oluştur
                try
                {
                    await _notificationService.NotifyPointsConvertedAsync(user.Id, id, reward.CostPoints);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bildirim gönderilemedi: {ex.Message}");
                }

                return Json(new { success = true, code, qrCode });
            }

            return Json(new { success = false, message = "Ödül kullanılamadı. Lütfen tekrar deneyin." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ödül kullanma hatası: {ex.Message}");
            return Json(new { success = false, message = "Ödül kullanma işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin." });
        }
    }
}



