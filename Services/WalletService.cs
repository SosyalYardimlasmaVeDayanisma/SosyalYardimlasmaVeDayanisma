using AfetPuan.Models;
using AfetPuan.Data;
using Microsoft.EntityFrameworkCore;

namespace AfetPuan.Services;

public class WalletService : IWalletService
{
    private readonly AppDbContext _context;

    public WalletService(AppDbContext context)
    {
        _context = context;
    }

    public Wallet GetWallet(string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return new Wallet
            {
                Balance = 0,
                Tier = WalletTier.Bronz,
                TotalEarned = 0,
                TotalSpent = 0,
                Transactions = new List<WalletTransaction>()
            };
        }

        var wallet = _context.Wallets
            .Include(w => w.Transactions)
            .FirstOrDefault(w => w.UserId == userId);

        if (wallet == null)
        {
            wallet = new Wallet
            {
                Id = $"wallet_{userId}_{Guid.NewGuid()}",
                UserId = userId,
                Balance = 0,
                Tier = WalletTier.Bronz,
                TotalEarned = 0,
                TotalSpent = 0,
                Transactions = new List<WalletTransaction>()
            };
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
        }
        else
        {
            // Transaction'ları tarihe göre sırala
            wallet.Transactions = wallet.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        return wallet;
    }

    public void AddTransaction(string userId, WalletTransaction transaction)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var wallet = GetWallet(userId);
        transaction.Id = $"tx_{Guid.NewGuid()}";
        transaction.CreatedAt = DateTime.Now;
        transaction.WalletId = wallet.Id;

        // ICollection için Add kullan
        _context.WalletTransactions.Add(transaction);
        wallet.Transactions.Add(transaction);
        
        wallet.Balance += transaction.Amount;

        if (transaction.Amount > 0)
            wallet.TotalEarned += transaction.Amount;
        else
            wallet.TotalSpent += Math.Abs(transaction.Amount);

        UpdateTier(wallet);
        _context.SaveChanges();
    }

    public void UpdateBalance(string userId, int amount)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var wallet = GetWallet(userId);
        wallet.Balance += amount;
        
        if (amount > 0)
            wallet.TotalEarned += amount;
        else
            wallet.TotalSpent += Math.Abs(amount);

        UpdateTier(wallet);
        _context.SaveChanges();
    }

    private void UpdateTier(Wallet wallet)
    {
        var totalPoints = wallet.TotalEarned;
        wallet.Tier = totalPoints switch
        {
            >= 10000 => WalletTier.Platin,
            >= 5000 => WalletTier.Altın,
            >= 1000 => WalletTier.Gümüş,
            _ => WalletTier.Bronz
        };
    }
}
