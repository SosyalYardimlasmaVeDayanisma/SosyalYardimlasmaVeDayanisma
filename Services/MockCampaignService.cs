using AfetPuan.Models;

namespace AfetPuan.Services;

public class MockCampaignService : ICampaignService
{
    private readonly List<Campaign> _campaigns = new()
    {
        new Campaign
        {
            Id = "1",
            Title = "Kahramanmaraş Deprem Yardım Kampanyası",
            DisasterType = DisasterType.Deprem,
            City = "Kahramanmaraş",
            GoalAmount = 5000000,
            RaisedAmount = 3250000,
            Status = CampaignStatus.Aktif,
            NgoName = "Türk Kızılayı",
            Description = "Kahramanmaraş ve çevresindeki depremzedelere acil yardım kampanyası",
            CreatedAt = new DateTime(2024, 1, 15, 10, 0, 0)
        },
        new Campaign
        {
            Id = "2",
            Title = "İstanbul Sel Yardım Fonu",
            DisasterType = DisasterType.Sel,
            City = "İstanbul",
            GoalAmount = 2000000,
            RaisedAmount = 1850000,
            Status = CampaignStatus.Aktif,
            NgoName = "AKUT",
            Description = "Sel felaketinden etkilenen ailelere destek",
            CreatedAt = new DateTime(2024, 2, 1, 8, 30, 0)
        },
        new Campaign
        {
            Id = "3",
            Title = "Muğla Orman Yangını Acil Yardım",
            DisasterType = DisasterType.Yangın,
            City = "Muğla",
            GoalAmount = 3000000,
            RaisedAmount = 2450000,
            Status = CampaignStatus.Aktif,
            NgoName = "TEMA Vakfı",
            Description = "Orman yangınından zarar gören bölgelere yardım",
            CreatedAt = new DateTime(2024, 3, 10, 14, 20, 0)
        },
        new Campaign
        {
            Id = "4",
            Title = "Ankara Kış Yardımı",
            DisasterType = DisasterType.Diğer,
            City = "Ankara",
            GoalAmount = 1500000,
            RaisedAmount = 1500000,
            Status = CampaignStatus.Tamamlandı,
            NgoName = "Kızılay",
            Description = "Soğuk kış günlerinde ihtiyaç sahiplerine yardım",
            CreatedAt = new DateTime(2024, 1, 1, 9, 0, 0)
        },
        new Campaign
        {
            Id = "5",
            Title = "Antalya Deprem Hazırlık Fonu",
            DisasterType = DisasterType.Deprem,
            City = "Antalya",
            GoalAmount = 4000000,
            RaisedAmount = 1200000,
            Status = CampaignStatus.Aktif,
            NgoName = "AFAD",
            Description = "Deprem hazırlık ve önleme çalışmaları",
            CreatedAt = new DateTime(2024, 3, 15, 11, 0, 0)
        }
    };

    public List<Campaign> GetAllCampaigns() => _campaigns;

    public Campaign? GetCampaignById(string id) => 
        _campaigns.FirstOrDefault(c => c.Id == id);

    public List<Campaign> FilterCampaigns(string? search, string? city, DisasterType? disasterType, CampaignStatus? status, CampaignCategory? category = null)
    {
        var filtered = _campaigns.AsEnumerable();

        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            filtered = filtered.Where(c => 
                c.Title.ToLower().Contains(searchLower) ||
                c.City.ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrEmpty(city))
        {
            filtered = filtered.Where(c => c.City == city);
        }

        if (disasterType.HasValue)
        {
            filtered = filtered.Where(c => c.DisasterType == disasterType.Value);
        }

        if (status.HasValue)
        {
            filtered = filtered.Where(c => c.Status == status.Value);
        }

        if (category.HasValue)
        {
            filtered = filtered.Where(c => c.Category == category.Value);
        }

        return filtered.ToList();
    }

    public List<Campaign> GetCampaignsByCategory(CampaignCategory? category)
    {
        var filtered = _campaigns.Where(c => c.Status == CampaignStatus.Aktif);
        
        if (category.HasValue)
        {
            filtered = filtered.Where(c => c.Category == category.Value);
        }

        return filtered
            .OrderByDescending(c => c.IsTimeSensitive)
            .ThenByDescending(c => c.CreatedAt)
            .ToList();
    }

    public List<Campaign> GetTimeSensitiveCampaigns()
    {
        var now = DateTime.UtcNow;
        return _campaigns
            .Where(c => c.IsTimeSensitive && 
                       c.Status == CampaignStatus.Aktif &&
                       (!c.TimeContextExpiry.HasValue || c.TimeContextExpiry.Value > now))
            .OrderByDescending(c => c.CreatedAt)
            .ToList();
    }

    public List<Campaign> GetFeaturedCampaigns()
    {
        return _campaigns
            .Where(c => c.IsFeatured && c.Status == CampaignStatus.Aktif)
            .OrderByDescending(c => c.Priority)
            .ThenByDescending(c => c.CreatedAt)
            .Take(6)
            .ToList();
    }

    public void UpdateRaisedAmount(string campaignId, decimal amount)
    {
        var campaign = GetCampaignById(campaignId);
        if (campaign != null)
        {
            campaign.RaisedAmount += amount;
            if (campaign.RaisedAmount >= campaign.GoalAmount && campaign.Status == CampaignStatus.Aktif)
            {
                campaign.Status = CampaignStatus.Tamamlandı;
            }
        }
    }
}



