using System.ComponentModel.DataAnnotations;

namespace SosyalYardim.Models;

public enum DisasterType
{
    Deprem,
    Sel,
    Yangın,
    Diğer
}

public enum CampaignStatus
{
    Aktif,
    Tamamlandı,
    Beklemede
}

public enum CampaignCategory
{
    AfetAcilDurum,
    EgitimCocuk,
    Saglik,
    Hayvanlar,
    GidaTemelIhtiyac,
    CevreVeDoga,
    EngelliDogumluBireyler,
    YasliDestegi,
    KulturSanat,
    HayalGerceklestirme,
    SporVeRekreasyon,
    TeknolojiveYenilikcilik
}

public class Campaign
{
    [Key]
    public string Id { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Kampanya başlığı gereklidir")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Başlık 10-200 karakter arasında olmalıdır")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public DisasterType DisasterType { get; set; }
    
    [Required]
    public CampaignCategory Category { get; set; } = CampaignCategory.AfetAcilDurum;
    
    [Required(ErrorMessage = "Şehir gereklidir")]
    [StringLength(50)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [Range(100, double.MaxValue, ErrorMessage = "Hedef tutar en az 100 TL olmalıdır")]
    public decimal GoalAmount { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal RaisedAmount { get; set; }
    
    [Required]
    public CampaignStatus Status { get; set; }
    
    [Required(ErrorMessage = "STK adı gereklidir")]
    [StringLength(100)]
    public string NgoName { get; set; } = string.Empty;
    
    [Url]
    public string? CoverUrl { get; set; }
    
    [StringLength(2000)]
    public string? Description { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    // Öne çıkan kampanya mı?
    public bool IsFeatured { get; set; } = false;
    
    [Range(0, 100)]
    public int Priority { get; set; } = 0;
    
    // Opsiyonel zaman bağlamı
    [StringLength(100)]
    public string? TimeContext { get; set; }
    
    public bool IsTimeSensitive { get; set; } = false;
    
    public DateTime? TimeContextExpiry { get; set; }

    public decimal Progress => GoalAmount > 0 ? (RaisedAmount / GoalAmount) * 100 : 0;
    
    public int PointsPreview => (int)(GoalAmount / 10);
    
    // Navigation Properties
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
    
    // Kategori için okunabilir isim
    public string CategoryDisplayName => Category switch
    {
        CampaignCategory.AfetAcilDurum => "Afet & Acil Durum",
        CampaignCategory.EgitimCocuk => "Eğitim & Çocuk",
        CampaignCategory.Saglik => "Sağlık",
        CampaignCategory.Hayvanlar => "Hayvanlar",
        CampaignCategory.GidaTemelIhtiyac => "Gıda & Temel İhtiyaç",
        CampaignCategory.CevreVeDoga => "Çevre & Doğa",
        CampaignCategory.EngelliDogumluBireyler => "Engelli & Doğumlu Bireyler",
        CampaignCategory.YasliDestegi => "Yaşlı Desteği",
        CampaignCategory.KulturSanat => "Kültür & Sanat",
        CampaignCategory.HayalGerceklestirme => "Hayal Gerçekleştirme",
        CampaignCategory.SporVeRekreasyon => "Spor & Rekreasyon",
        CampaignCategory.TeknolojiveYenilikcilik => "Teknoloji & Yenilikçilik",
        _ => "Diğer"
    };
}



