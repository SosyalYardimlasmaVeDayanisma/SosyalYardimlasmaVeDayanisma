using AfetPuan.Models;

namespace AfetPuan.Services;

public interface IWalletService
{
    Wallet GetWallet(string? userId = null);
    void AddTransaction(string userId, WalletTransaction transaction);
    void UpdateBalance(string userId, int amount);
}



