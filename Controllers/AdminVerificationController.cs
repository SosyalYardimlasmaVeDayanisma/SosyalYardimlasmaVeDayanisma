using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AfetPuan.Services;
using AfetPuan.Models;

namespace AfetPuan.Controllers;

[Authorize(Roles = "Admin")]
public class AdminVerificationController : Controller
{
    private readonly IImpactVerificationService _verificationService;
    private readonly ICampaignService _campaignService;

    public AdminVerificationController(IImpactVerificationService verificationService, ICampaignService campaignService)
    {
        _verificationService = verificationService;
        _campaignService = campaignService;
    }

    public async Task<IActionResult> Index()
    {
        var verifications = await _verificationService.GetVerificationsAsync("Campaign");
        return View(verifications);
    }

    public IActionResult Create(string entityId, string entityType)
    {
        var verification = new ImpactVerification
        {
            EntityId = entityId,
            EntityType = entityType
        };
        return View(verification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ImpactVerification verification)
    {
        if (ModelState.IsValid)
        {
            verification.VerificationDate = DateTime.Now;
            await _verificationService.CreateVerificationAsync(verification);
            TempData["Success"] = "Doğrulama başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
        return View(verification);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var verification = await _verificationService.GetVerificationAsync(id, "Campaign");
        if (verification == null) return NotFound();
        return View(verification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ImpactVerification verification)
    {
        if (ModelState.IsValid)
        {
            await _verificationService.UpdateVerificationAsync(verification);
            TempData["Success"] = "Doğrulama güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        return View(verification);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        await _verificationService.DeleteVerificationAsync(id);
        TempData["Success"] = "Doğrulama silindi.";
        return RedirectToAction(nameof(Index));
    }
}
