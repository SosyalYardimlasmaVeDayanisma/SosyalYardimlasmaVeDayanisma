using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SosyalYardim.Services;
using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Controllers;

[Authorize]
public class DonateController : Controller
{
    private readonly ICampaignService _campaignService;
    private readonly IWalletService _walletService;
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;

    public DonateController(ICampaignService campaignService, IWalletService walletService, UserManager<User> userManager, AppDbContext context, INotificationService notificationService, IEmailService emailService)
    {
        _campaignService = campaignService;
        _walletService = walletService;
        _userManager = userManager;
        _context = context;
        _notificationService = notificationService;
        _emailService = emailService;
    }

    public IActionResult Index(string? campaign)
    {
        var campaigns = _campaignService.GetAllCampaigns();
        Campaign? selectedCampaign = null;

        if (!string.IsNullOrEmpty(campaign))
        {
            selectedCampaign = _campaignService.GetCampaignById(campaign);
        }

        ViewBag.SelectedCampaign = selectedCampaign;
        ViewBag.Campaigns = campaigns;

        return View();
    }

    [HttpPost]
    [Route("/Donate/ProcessDonation")]
    public async Task<IActionResult> ProcessDonation([FromForm] string campaignId, [FromForm] decimal amount)
    {
        if (amount < 10)
        {
            return Json(new { success = false, message = "Minimum 10 TL bağış yapabilirsiniz" });
        }

        var campaign = _campaignService.GetCampaignById(campaignId);
        if (campaign == null)
        {
            return Json(new { success = false, message = "Kampanya bulunamadı" });
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Json(new { success = false, message = "Kullanıcı bulunamadı" });
        }

        // Database transaction ile işlemleri atomik yap
        using var dbTransaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var pointsEarned = (int)(amount / 10);
            
            var donation = new Donation
            {
                Id = $"donation_{Guid.NewGuid()}",
                UserId = user.Id,
                CampaignId = campaignId,
                Amount = amount,
                PointsEarned = pointsEarned,
                CreatedAt = DateTime.Now
            };

            var transaction = new WalletTransaction
            {
                Id = $"tx_{Guid.NewGuid()}",
                Type = TransactionType.Earn,
                Amount = pointsEarned,
                Ref = $"Kampanya #{campaignId}",
                CreatedAt = DateTime.Now,
                Description = campaign.Title
            };

            // Tüm işlemleri transaction içinde yap
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();
            
            _walletService.AddTransaction(user.Id, transaction);
            _campaignService.UpdateRaisedAmount(campaignId, amount);

            // Transaction commit
            await dbTransaction.CommitAsync();

            // Bildirim ve e-posta gönderimi (transaction dışında)
            try
            {
                await _notificationService.NotifyDonationMadeAsync(user.Id, campaignId, amount, pointsEarned);
            }
            catch (Exception ex)
            {
                // Bildirim hatası bağışı iptal etmemeli, sadece logla
                Console.WriteLine($"Bildirim gönderilemedi: {ex.Message}");
            }

            // E-posta gönder
            if (!string.IsNullOrEmpty(user.Email))
            {
                try
                {
                    await _emailService.SendDonationConfirmationEmailAsync(
                        user.Email,
                        user.FullName ?? "Değerli Bağışçı",
                        campaign.Title,
                        amount,
                        pointsEarned
                    );
                }
                catch (Exception ex)
                {
                    // E-posta hatası bağışı iptal etmemeli
                    Console.WriteLine($"E-posta gönderilemedi: {ex.Message}");
                }
            }

            return Json(new { 
                success = true, 
                pointsEarned = pointsEarned,
                transactionId = transaction.Id
            });
        }
        catch (Exception ex)
        {
            await dbTransaction.RollbackAsync();
            Console.WriteLine($"Bağış işlemi hatası: {ex.Message}");
            return Json(new { success = false, message = "Bağış işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin." });
        }
    }
}


