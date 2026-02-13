using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AfetPuan.Services;
using System.Security.Claims;

namespace AfetPuan.Controllers;

[Authorize]
public class ImpactController : Controller
{
    private readonly IImpactCardService _impactCardService;
    private readonly IPdfExportService _pdfExportService;

    public ImpactController(IImpactCardService impactCardService, IPdfExportService pdfExportService)
    {
        _impactCardService = impactCardService;
        _pdfExportService = pdfExportService;
    }

    public async Task<IActionResult> Card(string period = "monthly")
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var card = period switch
        {
            "total" => await _impactCardService.GenerateTotalCardAsync(userId),
            _ => await _impactCardService.GenerateMonthlyCardAsync(userId)
        };

        return View(card);
    }

    [HttpGet]
    public async Task<IActionResult> CustomCard(DateTime? startDate, DateTime? endDate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var start = startDate ?? DateTime.Now.AddMonths(-1);
        var end = endDate ?? DateTime.Now;

        var card = await _impactCardService.GenerateCustomPeriodCardAsync(userId, start, end);
        return View("Card", card);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadPdf(DateTime? startDate, DateTime? endDate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var start = startDate ?? DateTime.Now.AddMonths(-1);
        var end = endDate ?? DateTime.Now;

        var pdfBytes = await _pdfExportService.GenerateImpactReportAsync(userId, start, end);
        
        var fileName = $"Etki_Raporu_{start:yyyyMMdd}_{end:yyyyMMdd}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}
