using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SosyalYardim.Models;

namespace SosyalYardim.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ImpactVerification> ImpactVerifications { get; set; }
}


