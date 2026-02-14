using SosyalYardim.Models;

namespace SosyalYardim.Services;

public interface IPdfExportService
{
    Task<byte[]> GenerateImpactReportAsync(string userId, DateTime startDate, DateTime endDate);
}
