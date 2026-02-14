using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SosyalYardim.Models;

public enum TransactionType
{
    Earn,
    Spend,
    Decay,
    Match
}

public enum WalletTier
{
    Bronz,
    Gümüş,
    Altın,
    Platin
}

public class WalletTransaction
{
    [Key]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string WalletId { get; set; } = string.Empty;
    
    [Required]
    public TransactionType Type { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public string Ref { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public string? Description { get; set; }
    
    // Navigation Property
    [ForeignKey("WalletId")]
    public virtual Wallet? Wallet { get; set; }
}

public class Wallet
{
    [Key]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Balance { get; set; }
    
    [Required]
    public WalletTier Tier { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int TotalEarned { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int TotalSpent { get; set; }
    
    // Navigation Properties
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
    
    public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}



