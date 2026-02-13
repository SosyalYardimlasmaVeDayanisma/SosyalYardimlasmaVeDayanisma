using AfetPuan.Models;

namespace AfetPuan.Services;

public class MockTransparencyService : ITransparencyService
{
    public TransparencyStats GetStats()
    {
        return new TransparencyStats
        {
            TotalDonations = 9500000,
            TotalCampaigns = 5,
            TotalParticipants = 15234,
            AvgDonation = 623,
            CompletionRate = 0.87,
            MonthlyData = new List<MonthlyData>
            {
                new() { Month = "Ocak", Donations = 2100000, Campaigns = 2 },
                new() { Month = "Şubat", Donations = 3200000, Campaigns = 1 },
                new() { Month = "Mart", Donations = 4200000, Campaigns = 2 }
            }
        };
    }
}



