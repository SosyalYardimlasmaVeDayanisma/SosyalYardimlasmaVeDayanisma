using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SosyalYardim.Services;
using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Controllers;

[Authorize(Roles = "Admin")]
public class AdminCampaignController : Controller
{
    private readonly ICampaignService _campaignService;
    private readonly AppDbContext _context;

    public AdminCampaignController(ICampaignService campaignService, AppDbContext context)
    {
        _campaignService = campaignService;
        _context = context;
    }

    public IActionResult Index()
    {
        var campaigns = _campaignService.GetAllCampaigns();
        return View(campaigns);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Campaign campaign)
    {
        try
        {
            // Validation kontrolleri
            if (string.IsNullOrWhiteSpace(campaign.Title) || campaign.Title.Length < 10)
            {
                ViewBag.Error = "Kampanya adı en az 10 karakter olmalıdır";
                return View(campaign);
            }
            
            if (campaign.GoalAmount < 100)
            {
                ViewBag.Error = "Hedef tutar en az 100 TL olmalıdır";
                return View(campaign);
            }
            
            if (string.IsNullOrWhiteSpace(campaign.City))
            {
                ViewBag.Error = "Şehir bilgisi gereklidir";
                return View(campaign);
            }
            
            if (string.IsNullOrWhiteSpace(campaign.NgoName))
            {
                ViewBag.Error = "STK adı gereklidir";
                return View(campaign);
            }

            campaign.Id = $"campaign_{Guid.NewGuid()}";
            campaign.CreatedAt = DateTime.Now;
            campaign.RaisedAmount = 0;
            campaign.Status = CampaignStatus.Aktif;

            _context.Campaigns.Add(campaign);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kampanya başarıyla oluşturuldu!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kampanya oluşturma hatası: {ex.Message}");
            ViewBag.Error = "Kampanya oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.";
            return View(campaign);
        }
    }

    public IActionResult Edit(string id)
    {
        var campaign = _campaignService.GetCampaignById(id);
        if (campaign == null)
            return NotFound();

        return View(campaign);
    }

    [HttpPost]
    public IActionResult Edit(string id, Campaign campaign)
    {
        try
        {
            var existing = _context.Campaigns.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return NotFound();
            
            // Validation
            if (string.IsNullOrWhiteSpace(campaign.Title) || campaign.Title.Length < 10)
            {
                ViewBag.Error = "Kampanya adı en az 10 karakter olmalıdır";
                return View(existing);
            }
            
            if (campaign.GoalAmount < 100)
            {
                ViewBag.Error = "Hedef tutar en az 100 TL olmalıdır";
                return View(existing);
            }

            existing.Title = campaign.Title;
            existing.Description = campaign.Description;
            existing.City = campaign.City;
            existing.DisasterType = campaign.DisasterType;
            existing.GoalAmount = campaign.GoalAmount;
            existing.NgoName = campaign.NgoName;
            existing.Status = campaign.Status;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kampanya başarıyla güncellendi!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kampanya güncelleme hatası: {ex.Message}");
            ViewBag.Error = "Kampanya güncellenirken bir hata oluştu. Lütfen tekrar deneyin.";
            var existing = _context.Campaigns.FirstOrDefault(c => c.Id == id);
            return View(existing);
        }
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        var campaign = _context.Campaigns.FirstOrDefault(c => c.Id == id);
        if (campaign == null)
            return NotFound();

        _context.Campaigns.Remove(campaign);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Kampanya başarıyla silindi!";
        return RedirectToAction("Index");
    }
}
