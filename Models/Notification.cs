namespace AfetPuan.Models;

public enum NotificationType
{
    DonationMade,           // Katkı yapıldı
    PointsConverted,        // Puan seçeneğe dönüştürüldü
    CampaignCompleted,      // Desteklenen kampanya tamamlandı
    ImpactRealized          // Etki gerçekleşti
}

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // İlgili referanslar
    public string? RelatedEntityId { get; set; } // Campaign, Donation, Reward ID
    public string? RelatedEntityType { get; set; } // "Campaign", "Donation", "Reward"
    
    // Navigasyon
    public User? User { get; set; }
}
