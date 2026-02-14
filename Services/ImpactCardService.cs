using SosyalYardim.Models;
using SosyalYardim.Data;
using Microsoft.EntityFrameworkCore;

namespace SosyalYardim.Services;

public class ImpactCardService : IImpactCardService
{
    private readonly AppDbContext _context;

    public ImpactCardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ImpactCard> GenerateMonthlyCardAsync(string userId)
    {
        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = DateTime.Now;
        return await GenerateCustomPeriodCardAsync(userId, startDate, endDate);
    }

    public async Task<ImpactCard> GenerateCustomPeriodCardAsync(string userId, DateTime startDate, DateTime endDate)
    {
        var donations = await _context.Donations
            .Where(d => d.UserId == userId && d.CreatedAt >= startDate && d.CreatedAt <= endDate)
            .ToListAsync();

        var campaigns = await _context.Campaigns
            .Where(c => donations.Select(d => d.CampaignId).Contains(c.Id))
            .ToListAsync();

        var wallet = await _context.Wallets
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        var transactions = wallet?.Transactions
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToList() ?? new List<WalletTransaction>();

        var topCategories = campaigns
            .GroupBy(c => c.CategoryDisplayName)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToList();

        var totalDonations = donations.Count;
        var totalAmount = donations.Sum(d => d.Amount);
        var totalPoints = donations.Sum(d => d.PointsEarned);
        var rewardsRedeemed = transactions.Count(t => t.Type == TransactionType.Spend);

        var impactStatement = GenerateImpactStatement(totalDonations, totalAmount, campaigns.Count);

        return new ImpactCard
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate,
            TotalDonations = totalDonations,
            TotalAmount = totalAmount,
            TotalPoints = totalPoints,
            CampaignsSupported = campaigns.Select(c => c.Id).Distinct().Count(),
            RewardsRedeemed = rewardsRedeemed,
            ImpactStatement = impactStatement,
            TopCategories = topCategories
        };
    }

    public async Task<ImpactCard> GenerateTotalCardAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var startDate = user?.CreatedAt ?? DateTime.Now.AddYears(-1);
        var endDate = DateTime.Now;
        return await GenerateCustomPeriodCardAsync(userId, startDate, endDate);
    }

    private string GenerateImpactStatement(int donations, decimal amount, int campaigns)
    {
        if (donations == 0)
            return "Henüz katkı yapmadınız. İlk adımı atmaya hazır mısınız?";

        if (amount < 100)
            return $"{donations} katkı ile {campaigns} kampanyayı desteklediniz. Küçük adımlar büyük değişimler yaratır!";

        if (amount < 500)
            return $"{campaigns} farklı alana katkı sağladınız. Etkileriniz toplumda yankı buluyor!";

        if (amount < 1000)
            return $"{amount:C} değerinde destek sundunuz. Bu önemli bir etki! Teşekkürler.";

        return $"{amount:C} ile {campaigns} kampanyayı desteklediniz. Olağanüstü bir etki yarattınız!";
    }
}
