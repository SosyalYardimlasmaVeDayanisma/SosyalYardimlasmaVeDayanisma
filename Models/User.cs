using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AfetPuan.Models;

public class User : IdentityUser
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Gizlilik tercihleri
    public bool ShowImpactPublicly { get; set; } = true;
    public bool ShowAnonymously { get; set; } = false;
    
    // Navigation Properties
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}


