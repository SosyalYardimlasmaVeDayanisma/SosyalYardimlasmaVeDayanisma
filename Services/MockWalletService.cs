using SosyalYardim.Models;

namespace SosyalYardim.Services;

public class MockWalletService : IWalletService
{
    private readonly Dictionary<string, Wallet> _wallets = new();

    private Wallet GetOrCreateWallet(string userId)
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

        if (!_wallets.ContainsKey(userId))
        {
            _wallets[userId] = new Wallet
            {
                Balance = 0,
                Tier = WalletTier.Bronz,
                TotalEarned = 0,
                TotalSpent = 0,
                Transactions = new List<WalletTransaction>()
            };
        }

        return _wallets[userId];
    }

    public Wallet GetWallet(string? userId = null)
    {
        return GetOrCreateWallet(userId ?? string.Empty);
    }

    public void AddTransaction(string userId, WalletTransaction transaction)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var wallet = GetOrCreateWallet(userId);
        
        // ICollection için Add kullan - başa eklemek için önce listeye çevir
        var transactionsList = wallet.Transactions.ToList();
        transactionsList.Insert(0, transaction);
        wallet.Transactions = transactionsList;
        
        wallet.Balance += transaction.Amount;
        
        if (transaction.Amount > 0)
            wallet.TotalEarned += transaction.Amount;
        else
            wallet.TotalSpent += Math.Abs(transaction.Amount);

        UpdateTier(wallet);
    }

    public void UpdateBalance(string userId, int amount)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var wallet = GetOrCreateWallet(userId);
        wallet.Balance += amount;
        UpdateTier(wallet);
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



