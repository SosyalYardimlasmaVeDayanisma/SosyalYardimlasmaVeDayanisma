using AfetPuan.Models;

namespace AfetPuan.Services;

public interface IImpactCardService
{
    Task<ImpactCard> GenerateMonthlyCardAsync(string userId);
    Task<ImpactCard> GenerateCustomPeriodCardAsync(string userId, DateTime startDate, DateTime endDate);
    Task<ImpactCard> GenerateTotalCardAsync(string userId);
}
