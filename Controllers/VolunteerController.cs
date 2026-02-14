using Microsoft.AspNetCore.Mvc;
using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VolunteerController> _logger;

        public VolunteerController(AppDbContext context, ILogger<VolunteerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(VolunteerApplication application)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Başvuruyu veritabanına kaydet
                    application.Status = ApplicationStatus.Pending;
                    application.CreatedAt = DateTime.Now;
                    
                    _context.VolunteerApplications.Add(application);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Yeni gönüllü başvurusu kaydedildi: {application.Email}");

                    TempData["SuccessMessage"] = "Gönüllülük başvurunuz başarıyla alındı! En kısa sürede sizinle iletişime geçeceğiz.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Gönüllü başvurusu kaydedilirken hata oluştu");
                    TempData["ErrorMessage"] = "Başvurunuz kaydedilirken bir hata oluştu. Lütfen tekrar deneyiniz.";
                }
            }

            return View("Index", application);
        }
    }
}
