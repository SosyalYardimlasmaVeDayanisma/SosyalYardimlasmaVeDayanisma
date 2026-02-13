using AfetPuan.Models;
using AfetPuan.Data;
using Microsoft.EntityFrameworkCore;

namespace AfetPuan.Services;

public class CampaignService : ICampaignService
{
    private readonly AppDbContext _context;

    public CampaignService(AppDbContext context)
    {
        _context = context;
    }

    public List<Campaign> GetAllCampaigns() => 
        _context.Campaigns.OrderByDescending(c => c.Priority).ToList();

    public List<Campaign> GetFeaturedCampaigns() =>
        _context.Campaigns
            .Where(c => c.IsFeatured && c.Status == CampaignStatus.Aktif)
            .OrderByDescending(c => c.Priority)
            .Take(6)
            .ToList();

    public Campaign? GetCampaignById(string id) => 
        _context.Campaigns.FirstOrDefault(c => c.Id == id);

    public List<Campaign> FilterCampaigns(string? search, string? city, DisasterType? disasterType, CampaignStatus? status, CampaignCategory? category = null)
    {
        var query = _context.Campaigns.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(c => 
                c.Title.ToLower().Contains(searchLower) ||
                c.City.ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(c => c.City == city);
        }

        if (disasterType.HasValue)
        {
            query = query.Where(c => c.DisasterType == disasterType.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (category.HasValue)
        {
            query = query.Where(c => c.Category == category.Value);
        }

        return query.ToList();
    }

    public List<Campaign> GetCampaignsByCategory(CampaignCategory? category)
    {
        var query = _context.Campaigns.Where(c => c.Status == CampaignStatus.Aktif);
        
        if (category.HasValue)
        {
            query = query.Where(c => c.Category == category.Value);
        }

        // Zaman bağlamlı kampanyaları önce göster
        return query
            .OrderByDescending(c => c.IsTimeSensitive)
            .ThenByDescending(c => c.CreatedAt)
            .ToList();
    }

    public List<Campaign> GetTimeSensitiveCampaigns()
    {
        var now = DateTime.UtcNow;
        return _context.Campaigns
            .Where(c => c.IsTimeSensitive && 
                       c.Status == CampaignStatus.Aktif &&
                       (!c.TimeContextExpiry.HasValue || c.TimeContextExpiry.Value > now))
            .OrderByDescending(c => c.CreatedAt)
            .ToList();
    }

    public void UpdateRaisedAmount(string campaignId, decimal amount)
    {
        var campaign = _context.Campaigns.FirstOrDefault(c => c.Id == campaignId);
        if (campaign != null)
        {
            campaign.RaisedAmount += amount;
            if (campaign.RaisedAmount >= campaign.GoalAmount && campaign.Status == CampaignStatus.Aktif)
            {
                campaign.Status = CampaignStatus.Tamamlandı;
            }
            _context.SaveChanges();
        }
    }

    public void InitializeSampleData()
    {
        if (_context.Campaigns.Any())
            return;

        var campaigns = new List<Campaign>
        {
            new Campaign
            {
                Id = "1",
                Title = "Kahramanmaraş Deprem Yardımı",
                DisasterType = DisasterType.Deprem,
                Category = CampaignCategory.AfetAcilDurum,
                City = "Kahramanmaraş",
                GoalAmount = 5000000,
                RaisedAmount = 3750000,
                Status = CampaignStatus.Aktif,
                NgoName = "Türk Kızılayı",
                Description = "Kahramanmaraş ve çevresindeki depremzedelere acil yardım, barınak ve gıda desteği",
                CreatedAt = new DateTime(2025, 1, 15, 10, 0, 0),
                IsFeatured = true,
                Priority = 100,
                IsTimeSensitive = true,
                TimeContext = "Acil Durum"
            },
            new Campaign
            {
                Id = "2",
                Title = "Çocuklar İçin Eğitim Desteği",
                DisasterType = DisasterType.Diğer,
                Category = CampaignCategory.EgitimCocuk,
                City = "Ankara",
                GoalAmount = 2000000,
                RaisedAmount = 850000,
                Status = CampaignStatus.Aktif,
                NgoName = "UNICEF Türkiye",
                Description = "Maddi imkansızlık yaşayan çocukların okul, kırtasiye ve eğitim ihtiyaçlarını karşılıyoruz",
                CreatedAt = new DateTime(2025, 2, 1, 9, 0, 0),
                IsFeatured = true,
                Priority = 90
            },
            new Campaign
            {
                Id = "3",
                Title = "Sokak Hayvanları Bakım ve Tedavi",
                DisasterType = DisasterType.Diğer,
                Category = CampaignCategory.Hayvanlar,
                City = "İzmir",
                GoalAmount = 800000,
                RaisedAmount = 560000,
                Status = CampaignStatus.Aktif,
                NgoName = "Haytap",
                Description = "Sokak hayvanlarına mama, barınak, aşı ve tedavi desteği sağlıyoruz",
                CreatedAt = new DateTime(2025, 1, 20, 11, 0, 0),
                IsFeatured = true,
                Priority = 80
            },
            new Campaign
            {
                Id = "4",
                Title = "Sağlık Desteği - Kanser Tedavi Fonu",
                DisasterType = DisasterType.Diğer,
                Category = CampaignCategory.Saglik,
                City = "İstanbul",
                GoalAmount = 3500000,
                RaisedAmount = 2400000,
                Status = CampaignStatus.Aktif,
                NgoName = "Türkiye Kanser Derneği",
                Description = "Kanser tedavisi gören hastaların ilaç ve tedavi masraflarına destek oluyoruz",
                CreatedAt = new DateTime(2025, 1, 5, 14, 0, 0),
                IsFeatured = true,
                Priority = 85
            },
            new Campaign
            {
                Id = "5",
                Title = "TEMA ile Yeşil Gelecek - Fidan Dikimi",
                DisasterType = DisasterType.Diğer,
                Category = CampaignCategory.CevreVeDoga,
                City = "Türkiye Geneli",
                GoalAmount = 1500000,
                RaisedAmount = 980000,
                Status = CampaignStatus.Aktif,
                NgoName = "TEMA Vakfı",
                Description = "Ormansızlaşan bölgelere fidan dikimi, orman rehabilitasyonu ve çevre koruma çalışmaları",
                CreatedAt = new DateTime(2025, 2, 10, 9, 30, 0),
                IsFeatured = true,
                Priority = 75
            }
        };

        _context.Campaigns.AddRange(campaigns);
        _context.SaveChanges();
    }
}
