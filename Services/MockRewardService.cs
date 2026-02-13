using AfetPuan.Models;

namespace AfetPuan.Services;

public class MockRewardService : IRewardService
{
    // Mock servis artık kullanılmıyor - RewardService kullanılıyor
    private readonly List<Reward> _rewards = new();

    public List<Reward> GetAllRewards() => _rewards;

    public Reward? GetRewardById(string id) => 
        _rewards.FirstOrDefault(r => r.Id == id);

    public List<Reward> SearchRewards(string? search)
    {
        if (string.IsNullOrEmpty(search))
            return _rewards;

        var searchLower = search.ToLower();
        return _rewards.Where(r =>
            r.Title.ToLower().Contains(searchLower) ||
            r.MerchantName.ToLower().Contains(searchLower) ||
            r.Category.ToLower().Contains(searchLower)
        ).ToList();
    }

    public List<Reward> GetRewardsByCategory(RewardCategory? category)
    {
        var now = DateTime.Now;
        var filtered = _rewards.Where(r => r.ValidUntil > now && r.Stock > 0);
        
        if (category.HasValue)
        {
            filtered = filtered.Where(r => r.RewardType == category.Value);
        }

        return filtered
            .OrderBy(r => r.RewardType)
            .ThenBy(r => r.CostPoints)
            .ToList();
    }

    public List<Reward> GetRewardsByContributionType(ContributionType contributionType)
    {
        var now = DateTime.Now;
        var filtered = _rewards.Where(r => r.ValidUntil > now && r.Stock > 0);
        
        if (contributionType == ContributionType.BaskasıIcinDonustur)
        {
            filtered = filtered.Where(r => r.RewardType == RewardCategory.ToplumselKatki);
        }
        else
        {
            // Kendin için geliştir: Tüm kişisel gelişim kategorileri
            filtered = filtered.Where(r => r.RewardType == RewardCategory.YerelEtikFiziki || 
                                           r.RewardType == RewardCategory.DijitalOgrenme ||
                                           r.RewardType == RewardCategory.DijitalRehber ||
                                           r.RewardType == RewardCategory.ToplulukErisim ||
                                           r.RewardType == RewardCategory.UretkenlikAraci);
        }

        return filtered.OrderBy(r => r.CostPoints).ToList();
    }

    public List<Reward> GetSocialImpactRewards()
    {
        var now = DateTime.Now;
        return _rewards
            .Where(r => r.RewardType == RewardCategory.ToplumselKatki && 
                       r.ValidUntil > now && 
                       r.Stock > 0)
            .OrderBy(r => r.CostPoints)
            .ToList();
    }

    public (bool Success, string? Code, string? QrCode) RedeemReward(string rewardId)
    {
        var reward = GetRewardById(rewardId);
        if (reward == null || !reward.IsValid || !reward.InStock)
            return (false, null, null);

        reward.Stock--;

        var code = $"REDEEM-{DateTime.Now.Ticks}";
        var qrCode = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgZmlsbD0iIzIyQzU1RSIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LXNpemU9IjE4IiBmaWxsPSJ3aGl0ZSIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPkFGRVRQVUFOPC90ZXh0Pjwvc3ZnPg==";
        
        return (true, code, qrCode);
    }
}



