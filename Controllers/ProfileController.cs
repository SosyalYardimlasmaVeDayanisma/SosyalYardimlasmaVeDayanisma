using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SosyalYardim.Services;
using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IWalletService _walletService;
    private readonly AppDbContext _context;

    public ProfileController(UserManager<User> userManager, IWalletService walletService, AppDbContext context)
    {
        _userManager = userManager;
        _walletService = walletService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Auth");

        var wallet = _walletService.GetWallet(user.Id);
        
        // Kullanıcının bağışları - Campaign ilişkisini Include ile çek (N+1 problemi çözümü)
        var donations = _context.Donations
            .Include(d => d.Campaign)
            .Where(d => d.UserId == user.Id)
            .OrderByDescending(d => d.CreatedAt)
            .ToList();

        // Kampanya bilgileri - Artık Include ile geldiği için tekrar sorguya gerek yok
        var campaigns = donations
            .Where(d => d.Campaign != null)
            .Select(d => d.Campaign!)
            .Distinct()
            .ToList();
        
        // İyilik alanları gruplandırması
        var impactAreas = campaigns
            .GroupBy(c => c.Category)
            .Select(g => new { 
                Category = g.Key, 
                Count = g.Count(),
                CategoryName = g.First().CategoryDisplayName
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        // Cüzdan işlemleri
        var transactions = _context.WalletTransactions
            .Where(t => t.WalletId == wallet.Id)
            .OrderByDescending(t => t.CreatedAt)
            .Take(10)
            .ToList();

        ViewBag.User = user;
        ViewBag.Wallet = wallet;
        ViewBag.Donations = donations;
        ViewBag.Campaigns = campaigns;
        ViewBag.ImpactAreas = impactAreas;
        ViewBag.DonationCount = donations.Count;
        ViewBag.UniqueAreasCount = impactAreas.Count;
        ViewBag.Transactions = transactions;
        ViewBag.TotalDonations = donations.Sum(d => d.Amount);

        return View(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdatePrivacy(bool showImpactPublicly, bool showAnonymously)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Auth");

        user.ShowImpactPublicly = showImpactPublicly;
        user.ShowAnonymously = showAnonymously;
        
        await _userManager.UpdateAsync(user);
        
        TempData["SuccessMessage"] = "Gizlilik tercihlerin güncellendi.";
        return RedirectToAction("Index");
    }
}
