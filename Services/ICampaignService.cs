using AfetPuan.Models;

namespace AfetPuan.Services;

public interface ICampaignService
{
    List<Campaign> GetAllCampaigns();
    List<Campaign> GetFeaturedCampaigns();
    Campaign? GetCampaignById(string id);
    List<Campaign> FilterCampaigns(string? search, string? city, DisasterType? disasterType, CampaignStatus? status, CampaignCategory? category = null);
    List<Campaign> GetCampaignsByCategory(CampaignCategory? category);
    List<Campaign> GetTimeSensitiveCampaigns();
    void UpdateRaisedAmount(string campaignId, decimal amount);
}



