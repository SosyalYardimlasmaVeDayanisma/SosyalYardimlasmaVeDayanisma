using AfetPuan.Models;

namespace AfetPuan.Services;

public interface IPdfExportService
{
    Task<byte[]> GenerateImpactReportAsync(string userId, DateTime startDate, DateTime endDate);
}
