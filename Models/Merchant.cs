namespace AfetPuan.Models;

public enum MerchantType
{
    YerelFiziki,           // Yerel Fiziki İşletmeler
    KooperatifSosyal,      // Kooperatif ve Sosyal Girişimler
    YerliDijital           // Yerli ve Etik Dijital Destekçiler
}

public class Merchant
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public MerchantType Type { get; set; } = MerchantType.YerelFiziki;
    public string BusinessType { get; set; } = string.Empty; // "Kahve Dükkanı", "Kooperatif", "Platform" vb.
    public string ContributionDescription { get; set; } = string.Empty; // Ekosisteme katkı açıklaması
    public bool IsLocal { get; set; } = true;
    public bool IsEthical { get; set; } = true;
    
    // Eski field - geriye dönük uyumluluk için
    public int RewardCount { get; set; }
    
    public string TypeDisplayName => Type switch
    {
        MerchantType.YerelFiziki => "Yerel Fiziki İşletme",
        MerchantType.KooperatifSosyal => "Kooperatif ve Sosyal Girişim",
        MerchantType.YerliDijital => "Yerli ve Etik Dijital Destekçi",
        _ => "Destekçi"
    };
}



