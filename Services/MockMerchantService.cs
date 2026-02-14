using SosyalYardim.Models;

namespace SosyalYardim.Services;

public class MockMerchantService : IMerchantService
{
    private readonly List<Merchant> _merchants;

    public MockMerchantService()
    {
        _merchants = new List<Merchant>
        {
            // Yerel Fiziki İşletmeler
            new Merchant 
            { 
                Id = "m1", 
                Name = "Şimdi Kahve", 
                City = "İstanbul - Beşiktaş", 
                Category = "Kahve & Çay",
                Type = MerchantType.YerelFiziki,
                BusinessType = "Özel Kahve Dükkanı",
                ContributionDescription = "Yerel kahve kültürünü destekliyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m2", 
                Name = "Karabatak Yayınevi", 
                City = "Ankara", 
                Category = "Kitap & Yayın",
                Type = MerchantType.YerelFiziki,
                BusinessType = "Bağımsız Yayınevi",
                ContributionDescription = "Yerli yazarları destekliyor",
                IsLocal = true,
                IsEthical = true
            },
            
            // Kooperatif ve Sosyal Girişimler
            new Merchant 
            { 
                Id = "m3", 
                Name = "TEMA Vakfı", 
                City = "Türkiye Geneli", 
                Category = "Çevre",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "Çevre Koruma Vakfı",
                ContributionDescription = "Fidan dikimi ve erozyon önleme",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m4", 
                Name = "Haytap", 
                City = "Türkiye Geneli", 
                Category = "Hayvan Hakları",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "Hayvan Hakları Derneği",
                ContributionDescription = "Sokak hayvanlarına destek",
                IsLocal = true,
                IsEthical = true
            },
            
            // Yerli ve Etik Dijital Destekçiler
            new Merchant 
            { 
                Id = "m5", 
                Name = "BTK Akademi", 
                City = "Online", 
                Category = "Teknoloji Eğitimi",
                Type = MerchantType.YerliDijital,
                BusinessType = "Kamu Eğitim Platformu",
                ContributionDescription = "Ücretsiz yazılım eğitimi",
                IsLocal = true,
                IsEthical = true
            }
        };
    }

    public List<Merchant> GetAllMerchants() => 
        _merchants.Where(m => m.IsLocal && m.IsEthical)
                  .OrderBy(m => m.Type)
                  .ThenBy(m => m.Name)
                  .ToList();

    public List<Merchant> GetMerchantsByType(MerchantType type) =>
        _merchants.Where(m => m.Type == type && m.IsLocal && m.IsEthical)
                  .OrderBy(m => m.Name)
                  .ToList();

    public Dictionary<MerchantType, List<Merchant>> GetMerchantsGroupedByType()
    {
        var merchants = GetAllMerchants();
        return merchants
            .GroupBy(m => m.Type)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}

