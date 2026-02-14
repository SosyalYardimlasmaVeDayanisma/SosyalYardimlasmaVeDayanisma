using SosyalYardim.Models;

namespace SosyalYardim.Services;

public interface IRewardService
{
    List<Reward> GetAllRewards();
    Reward? GetRewardById(string id);
    List<Reward> SearchRewards(string? search);
    List<Reward> GetRewardsByCategory(RewardCategory? category);
    List<Reward> GetRewardsByContributionType(ContributionType contributionType);
    List<Reward> GetSocialImpactRewards();
    (bool Success, string? Code, string? QrCode) RedeemReward(string rewardId);
}



