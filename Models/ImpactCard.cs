namespace SosyalYardim.Models;

public class ImpactCard
{
    public string UserId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int TotalDonations { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalPoints { get; set; }
    public int CampaignsSupported { get; set; }
    public int RewardsRedeemed { get; set; }
    
    public string ImpactStatement { get; set; } = string.Empty;
    public List<string> TopCategories { get; set; } = new();
    
    public string PeriodDescription => GetPeriodDescription();
    
    private string GetPeriodDescription()
    {
        if ((EndDate - StartDate).TotalDays <= 31)
            return "Aylık";
        if ((EndDate - StartDate).TotalDays <= 93)
            return "Üç Aylık";
        if ((EndDate - StartDate).TotalDays <= 366)
            return "Yıllık";
        return "Toplam";
    }
}
