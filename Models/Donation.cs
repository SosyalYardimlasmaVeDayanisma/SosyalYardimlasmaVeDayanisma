using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfetPuan.Models;

public class Donation
{
    [Key]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public string CampaignId { get; set; } = string.Empty;
    
    [Required]
    [Range(10, double.MaxValue, ErrorMessage = "Minimum bağış tutarı 10 TL olmalıdır")]
    public decimal Amount { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int PointsEarned { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public string? TransactionRef { get; set; }
    
    // Navigation Properties
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
    
    [ForeignKey("CampaignId")]
    public virtual Campaign? Campaign { get; set; }
}
