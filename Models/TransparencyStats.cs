namespace AfetPuan.Models;

public class MonthlyData
{
    public string Month { get; set; } = string.Empty;
    public decimal Donations { get; set; }
    public int Campaigns { get; set; }
}

public class TransparencyStats
{
    public decimal TotalDonations { get; set; }
    public int TotalCampaigns { get; set; }
    public int TotalParticipants { get; set; }
    public decimal AvgDonation { get; set; }
    public double CompletionRate { get; set; }
    public List<MonthlyData> MonthlyData { get; set; } = new();
}



