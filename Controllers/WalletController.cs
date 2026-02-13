using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AfetPuan.Services;
using AfetPuan.Models;

namespace AfetPuan.Controllers;

[Authorize]
public class WalletController : Controller
{
    private readonly IWalletService _walletService;
    private readonly UserManager<User> _userManager;

    public WalletController(IWalletService walletService, UserManager<User> userManager)
    {
        _walletService = walletService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? filter)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var wallet = _walletService.GetWallet(user.Id);

        var transactions = filter switch
        {
            "earn" => wallet.Transactions.Where(t => t.Amount > 0).ToList(),
            "spend" => wallet.Transactions.Where(t => t.Amount < 0).ToList(),
            _ => wallet.Transactions
        };

        ViewBag.Filter = filter;
        ViewBag.Transactions = transactions;

        return View(wallet);
    }
}



