namespace SosyalYardim.Models;

public enum ContributionType
{
    BaskasıIcinDonustur,   // Toplumsal etki: Fidan, mama, kitap, gıda kolisi vb.
    KendinIcinGelistir     // Kişisel gelişim, öğrenme ve bilinç odaklı içerikler
}

public enum RewardCategory
{
    ToplumselKatki,           // Fidan, mama, kitap, gıda kolisi vb.
    YerelEtikFiziki,          // Yerel küçük işletmeler
    DijitalOgrenme,           // Online kurslar, eğitim platformları
    DijitalRehber,            // E-kitaplar, rehberler, planlama araçları
    ToplulukErisim,           // Özel topluluklar, mentorluk programları
    UretkenlikAraci           // Üretkenlik uygulamaları, araçları
}

public class Reward
{
    public string Id { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public ContributionType ContributionType { get; set; } = ContributionType.BaskasıIcinDonustur;
    public RewardCategory RewardType { get; set; } = RewardCategory.ToplumselKatki;
    public string Title { get; set; } = string.Empty;
    public int CostPoints { get; set; }
    public int Stock { get; set; }
    public DateTime ValidUntil { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    // Etik ve yerel bilgiler
    public bool IsLocalBusiness { get; set; } = true;
    public bool IsEthical { get; set; } = true;
    public string? ImpactDescription { get; set; } // Toplumsal veya kişisel etki açıklaması
    public string? BusinessScale { get; set; } // "Küçük işletme", "Kooperatif" vb.
    
    // Kişisel gelişim odaklı alanlar
    public string? LearningOutcome { get; set; } // "Bu içerikle neleri öğreneceksiniz"
    public string? GrowthArea { get; set; } // "Bilinç", "Üretkenlik", "İletişim" vb.

    public bool IsValid => ValidUntil > DateTime.Now;
    public bool InStock => Stock > 0;
    
    // Kategori için okunabilir isim
    public string RewardTypeDisplayName => RewardType switch
    {
        RewardCategory.ToplumselKatki => "Toplumsal Etki",
        RewardCategory.YerelEtikFiziki => "Yerel İşletme",
        RewardCategory.DijitalOgrenme => "Dijital Öğrenme",
        RewardCategory.DijitalRehber => "Dijital Rehber",
        RewardCategory.ToplulukErisim => "Topluluk Erişimi",
        RewardCategory.UretkenlikAraci => "Üretkenlik Aracı",
        _ => "Diğer"
    };
    
    // Ana sekme belirleme (Başkası için / Kendin için)
    public ContributionType ContributionTab => RewardType == RewardCategory.ToplumselKatki 
        ? ContributionType.BaskasıIcinDonustur 
        : ContributionType.KendinIcinGelistir;
        
    public string ContributionTabDisplayName => ContributionTab switch
    {
        ContributionType.BaskasıIcinDonustur => "Başkası İçin Dönüştür",
        ContributionType.KendinIcinGelistir => "Kendin İçin Geliştir",
        _ => ""
    };
}



