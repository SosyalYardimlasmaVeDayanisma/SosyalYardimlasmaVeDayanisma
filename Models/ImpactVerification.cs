namespace AfetPuan.Models;

public class ImpactVerification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EntityId { get; set; } = string.Empty; // Campaign veya Donation ID
    public string EntityType { get; set; } = string.Empty; // "Campaign" veya "Donation"
    
    public bool IsVerified { get; set; } = false;
    public string? VerifyingOrganization { get; set; } // Doğrulayan kurum/STK
    public DateTime? VerificationDate { get; set; }
    public string? Description { get; set; } // Kısa açıklama
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}
