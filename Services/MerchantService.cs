using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Services;

public class MerchantService : IMerchantService
{
    private readonly AppDbContext _context;

    public MerchantService(AppDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
        InitializeSampleData();
    }

    public List<Merchant> GetAllMerchants()
    {
        return _context.Merchants
            .Where(m => m.IsLocal && m.IsEthical)
            .OrderBy(m => m.Type)
            .ThenBy(m => m.Name)
            .ToList();
    }

    public List<Merchant> GetMerchantsByType(MerchantType type)
    {
        return _context.Merchants
            .Where(m => m.Type == type && m.IsLocal && m.IsEthical)
            .OrderBy(m => m.Name)
            .ToList();
    }

    public Dictionary<MerchantType, List<Merchant>> GetMerchantsGroupedByType()
    {
        var merchants = GetAllMerchants();
        return merchants
            .GroupBy(m => m.Type)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    private void InitializeSampleData()
    {
        if (_context.Merchants.Any())
            return;

        var merchants = new List<Merchant>
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
                ContributionDescription = "Yerel kahve kültürünü destekliyor, bağış puanları için özel filtre kahve sunuyor",
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
                ContributionDescription = "Yerli yazarları destekliyor, bağış yapan okuyuculara özel seçki kitaplar sunuyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m3", 
                Name = "Sıfır Atık Mağazası", 
                City = "İstanbul - Kadıköy", 
                Category = "Sürdürülebilir Yaşam",
                Type = MerchantType.YerelFiziki,
                BusinessType = "Çevre Dostu Mağaza",
                ContributionDescription = "Plastik kullanımını azaltıyor, iyilik puanlarıyla bambu ürünler sunuyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m4", 
                Name = "Mahalle Kitap Kafe", 
                City = "İzmir - Alsancak", 
                Category = "Kafe & Kitap",
                Type = MerchantType.YerelFiziki,
                BusinessType = "Bağımsız Kitap Kafe",
                ContributionDescription = "Okuma kültürünü yaygınlaştırıyor, bağış yapanlara kitap okuma seansları düzenliyor",
                IsLocal = true,
                IsEthical = true
            },
            
            // Kooperatif ve Sosyal Girişimler
            new Merchant 
            { 
                Id = "m5", 
                Name = "Doğal Yaşam Kooperatifi", 
                City = "İzmir", 
                Category = "Organik Ürün",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "Üretici Kooperatifi",
                ContributionDescription = "Yerel üreticileri destekliyor, bağışçılara organik sebze paketleri sunuyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m6", 
                Name = "TEMA Vakfı", 
                City = "Türkiye Geneli", 
                Category = "Çevre",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "Çevre Koruma Vakfı",
                ContributionDescription = "Bağış puanlarını fidan dikimine dönüştürüyor, erozyon önleme çalışmaları yapıyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m7", 
                Name = "Haytap", 
                City = "Türkiye Geneli", 
                Category = "Hayvan Hakları",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "Hayvan Hakları Derneği",
                ContributionDescription = "Sokak hayvanlarına mama desteği sağlıyor, iyilik puanlarıyla katkı kabul ediyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m8", 
                Name = "Eğitim Gönüllüleri Vakfı", 
                City = "Türkiye Geneli", 
                Category = "Eğitim",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "Eğitim Destek Vakfı",
                ContributionDescription = "Kırsal bölgelere kitap götürüyor, bağış puanlarıyla eğitim katkısı yapılabiliyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m9", 
                Name = "Türk Kızılayı", 
                City = "Türkiye Geneli", 
                Category = "İnsani Yardım",
                Type = MerchantType.KooperatifSosyal,
                BusinessType = "İnsani Yardım Derneği",
                ContributionDescription = "Gıda kolisi desteği sağlıyor, afet yardımlarında iyilik puanı entegrasyonu sunuyor",
                IsLocal = true,
                IsEthical = true
            },
            
            // Yerli ve Etik Dijital Destekçiler
            new Merchant 
            { 
                Id = "m10", 
                Name = "BTK Akademi", 
                City = "Online", 
                Category = "Teknoloji Eğitimi",
                Type = MerchantType.YerliDijital,
                BusinessType = "Kamu Eğitim Platformu",
                ContributionDescription = "Ücretsiz yazılım eğitimi veriyor, bağış yapanlara sertifikalı kurs erişimi sunuyor",
                IsLocal = true,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m11", 
                Name = "Khan Academy Türkçe", 
                City = "Online", 
                Category = "Eğitim",
                Type = MerchantType.YerliDijital,
                BusinessType = "Kar Amacı Gütmeyen Platform",
                ContributionDescription = "Ücretsiz eğitim içeriği sunuyor, iyilik puanlarıyla premium erişim sağlıyor",
                IsLocal = false,
                IsEthical = true
            },
            new Merchant 
            { 
                Id = "m12", 
                Name = "Udemy Türkiye", 
                City = "Online", 
                Category = "Kişisel Gelişim",
                Type = MerchantType.YerliDijital,
                BusinessType = "Eğitim Platformu",
                ContributionDescription = "Gelişim odaklı kurslar sunuyor, bağışçılara seçili kurslar için indirim sağlıyor",
                IsLocal = false,
                IsEthical = true
            }
        };

        _context.Merchants.AddRange(merchants);
        _context.SaveChanges();
    }
}
