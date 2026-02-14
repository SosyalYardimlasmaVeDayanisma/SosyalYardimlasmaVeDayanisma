using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SosyalYardim.Data;
using SosyalYardim.Models;
using SosyalYardim.Services;
using Microsoft.EntityFrameworkCore;

namespace SosyalYardim.Controllers;

[Authorize(Roles = "Admin")]
public class AdminVolunteerController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<AdminVolunteerController> _logger;
    private readonly IEmailService _emailService;

    public AdminVolunteerController(AppDbContext context, ILogger<AdminVolunteerController> logger, IEmailService emailService)
    {
        _context = context;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        var applications = await _context.VolunteerApplications
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return View(applications);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, ApplicationStatus status, string? adminNotes)
    {
        try
        {
            var application = await _context.VolunteerApplications.FindAsync(id);
            if (application == null)
            {
                return Json(new { success = false, message = "Başvuru bulunamadı" });
            }

            application.Status = status;
            application.UpdatedAt = DateTime.Now;
            application.AdminNotes = adminNotes;

            await _context.SaveChangesAsync();

            // Kullanıcıya e-posta gönder
            var statusText = status switch
            {
                ApplicationStatus.Approved => "Onaylandı",
                ApplicationStatus.Rejected => "Reddedildi",
                ApplicationStatus.Pending => "Beklemede",
                _ => "Güncellendi"
            };

            if (!string.IsNullOrEmpty(application.Email))
            {
                await _emailService.SendVolunteerApplicationStatusEmailAsync(
                    application.Email,
                    application.FullName,
                    statusText,
                    adminNotes
                );
            }

            _logger.LogInformation($"Gönüllü başvurusu güncellendi: ID={id}, Durum={status}");

            return Json(new { success = true, message = "Başvuru durumu güncellendi ve kullanıcıya e-posta gönderildi" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Başvuru durumu güncellenirken hata oluştu");
            return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var application = await _context.VolunteerApplications.FindAsync(id);
            if (application == null)
            {
                return Json(new { success = false, message = "Başvuru bulunamadı" });
            }

            _context.VolunteerApplications.Remove(application);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Gönüllü başvurusu silindi: ID={id}");

            return Json(new { success = true, message = "Başvuru silindi" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Başvuru silinirken hata oluştu");
            return Json(new { success = false, message = "Silme sırasında bir hata oluştu" });
        }
    }
}
