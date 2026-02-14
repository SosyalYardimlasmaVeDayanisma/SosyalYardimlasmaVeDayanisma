using SosyalYardim.Models;
using SosyalYardim.Data;

namespace SosyalYardim.Services;

public class RewardService : IRewardService
{
    private readonly AppDbContext _context;

    public RewardService(AppDbContext context)
    {
        _context = context;
    }

    public List<Reward> GetAllRewards() => 
        _context.Rewards.ToList();

    public Reward? GetRewardById(string id) => 
        _context.Rewards.FirstOrDefault(r => r.Id == id);

    public List<Reward> SearchRewards(string? search)
    {
        var query = _context.Rewards.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(r =>
                r.Title.ToLower().Contains(searchLower) ||
                r.MerchantName.ToLower().Contains(searchLower) ||
                r.Category.ToLower().Contains(searchLower)
            );
        }

        return query.ToList();
    }

    public List<Reward> GetRewardsByCategory(RewardCategory? category)
    {
        var now = DateTime.Now;
        var query = _context.Rewards.Where(r => r.ValidUntil > now && r.Stock > 0);
        
        if (category.HasValue)
        {
            query = query.Where(r => r.RewardType == category.Value);
        }

        // Toplumsal katkı ödüllerini önce göster
        return query
            .OrderBy(r => r.RewardType)
            .ThenBy(r => r.CostPoints)
            .ToList();
    }

    public List<Reward> GetRewardsByContributionType(ContributionType contributionType)
    {
        var now = DateTime.Now;
        var query = _context.Rewards.Where(r => r.ValidUntil > now && r.Stock > 0);
        
        if (contributionType == ContributionType.BaskasıIcinDonustur)
        {
            // Toplumsal etki seçenekleri
            query = query.Where(r => r.RewardType == RewardCategory.ToplumselKatki);
        }
        else
        {
            // Kendin için geliştir: Tüm kişisel gelişim kategorileri
            query = query.Where(r => r.RewardType == RewardCategory.YerelEtikFiziki || 
                                    r.RewardType == RewardCategory.DijitalOgrenme ||
                                    r.RewardType == RewardCategory.DijitalRehber ||
                                    r.RewardType == RewardCategory.ToplulukErisim ||
                                    r.RewardType == RewardCategory.UretkenlikAraci);
        }

        return query
            .OrderBy(r => r.CostPoints)
            .ToList();
    }

    public List<Reward> GetSocialImpactRewards()
    {
        var now = DateTime.Now;
        return _context.Rewards
            .Where(r => r.RewardType == RewardCategory.ToplumselKatki && 
                       r.ValidUntil > now && 
                       r.Stock > 0)
            .OrderBy(r => r.CostPoints)
            .ToList();
    }

    public (bool Success, string? Code, string? QrCode) RedeemReward(string rewardId)
    {
        using var transaction = _context.Database.BeginTransaction();
        
        try
        {
            // Reward'ı kilitle ve tekrar kontrol et
            var reward = _context.Rewards
                .Where(r => r.Id == rewardId)
                .FirstOrDefault();
            
            if (reward == null || !reward.IsValid || reward.Stock <= 0)
            {
                transaction.Rollback();
                return (false, null, null);
            }

            // Stock'u azalt
            reward.Stock--;
            _context.SaveChanges();
            
            // Transaction'ı commit et
            transaction.Commit();

            var code = $"REDEEM-{DateTime.Now.Ticks}";
            var qrCode = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMjAwIiBoZWlnaHQ9IjIwMCIgZmlsbD0iIzIyQzU1RSIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LXNpemU9IjE4IiBmaWxsPSJ3aGl0ZSIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPkFGRVRQVUFOPC90ZXh0Pjwvc3ZnPg==";
            
            return (true, code, qrCode);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Ödül kullanma hatası: {ex.Message}");
            return (false, null, null);
        }
    }

    public void InitializeSampleData()
    {
        if (_context.Rewards.Any())
            return;

        var rewards = new List<Reward>
        {
            // Toplumsal ve Doğa Odaklı Katkılar
            new Reward
            {
                Id = "1",
                MerchantName = "TEMA Vakfı",
                City = "Türkiye Geneli",
                Category = "Çevre",
                RewardType = RewardCategory.ToplumselKatki,
                Title = "5 Fidan Dikimi",
                CostPoints = 250,
                Stock = 100,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "Adınıza 5 fidan dikilecek ve sertifikanız gönderilecektir",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "5 fidan, yıllık 50 kg CO2 emilimi sağlar",
                BusinessScale = "Vakıf"
            },
            new Reward
            {
                Id = "2",
                MerchantName = "Haytap",
                City = "Türkiye Geneli",
                Category = "Hayvan Hakları",
                RewardType = RewardCategory.ToplumselKatki,
                Title = "1 Aylık Sokak Hayvanı Mama Desteği",
                CostPoints = 300,
                Stock = 80,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "Bir sokak hayvanının 1 aylık mama ihtiyacı karşılanacaktır",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "1 can için sürdürülebilir yaşam desteği",
                BusinessScale = "Dernek"
            },
            new Reward
            {
                Id = "3",
                MerchantName = "Eğitim Gönüllüleri Vakfı",
                City = "Türkiye Geneli",
                Category = "Eğitim",
                RewardType = RewardCategory.ToplumselKatki,
                Title = "Köy Okuluna 3 Kitap Bağışı",
                CostPoints = 200,
                Stock = 120,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "Kırsal bölge okullarına 3 adet çocuk kitabı gönderilecektir",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "3 çocuğun okuma alışkanlığına katkı",
                BusinessScale = "Vakıf"
            },
            new Reward
            {
                Id = "4",
                MerchantName = "Türk Kızılayı",
                City = "Türkiye Geneli",
                Category = "Gıda",
                RewardType = RewardCategory.ToplumselKatki,
                Title = "1 Aileye Gıda Kolisi",
                CostPoints = 400,
                Stock = 60,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "İhtiyaç sahibi bir aileye temel gıda kolisi ulaştırılacaktır",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "1 ailenin 1 haftalık temel gıda ihtiyacı",
                BusinessScale = "Dernek"
            },
            new Reward
            {
                Id = "5",
                MerchantName = "Lösev",
                City = "Türkiye Geneli",
                Category = "Sağlık",
                RewardType = RewardCategory.ToplumselKatki,
                Title = "Çocuk Hastasına Oyuncak Desteği",
                CostPoints = 150,
                Stock = 90,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "Tedavi gören bir çocuğa umut oyuncağı hediyesi",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Bir çocuğun tedavi sürecine moral desteği",
                BusinessScale = "Vakıf"
            },
            
            // Yerel ve Etik Fiziki Ödüller
            new Reward
            {
                Id = "6",
                MerchantName = "Şimdi Kahve - Beşiktaş",
                City = "İstanbul",
                Category = "Yerel Kahve",
                RewardType = RewardCategory.YerelEtikFiziki,
                Title = "Özel Filtre Kahve",
                CostPoints = 180,
                Stock = 50,
                ValidUntil = new DateTime(2025, 6, 30),
                Description = "Beşiktaş'taki yerel kahvecide kullanılabilir",
                IsLocalBusiness = true,
                IsEthical = true,
                BusinessScale = "Küçük işletme"
            },
            new Reward
            {
                Id = "7",
                MerchantName = "Doğal Yaşam Kooperatifi",
                City = "İzmir",
                Category = "Organik Ürün",
                RewardType = RewardCategory.YerelEtikFiziki,
                Title = "Organik Sebze Paketi (2kg)",
                CostPoints = 280,
                Stock = 40,
                ValidUntil = new DateTime(2025, 6, 30),
                Description = "Yerel üreticiden mevsim sebzeleri",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Yerel tarımı destekler",
                BusinessScale = "Kooperatif"
            },
            new Reward
            {
                Id = "8",
                MerchantName = "Karabatak Yayınevi",
                City = "Ankara",
                Category = "Yerel Yayın",
                RewardType = RewardCategory.YerelEtikFiziki,
                Title = "Yerli Yazar Kitabı",
                CostPoints = 220,
                Stock = 35,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "Türk yazarlardan seçkin eserler",
                IsLocalBusiness = true,
                IsEthical = true,
                BusinessScale = "Küçük yayınevi"
            },
            new Reward
            {
                Id = "9",
                MerchantName = "Sıfır Atık Mağazası",
                City = "İstanbul",
                Category = "Sürdürülebilir Yaşam",
                RewardType = RewardCategory.YerelEtikFiziki,
                Title = "Bambu Diş Fırçası Seti",
                CostPoints = 120,
                Stock = 70,
                ValidUntil = new DateTime(2025, 12, 31),
                Description = "Çevre dostu günlük kullanım ürünleri",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Plastik atığı azaltır",
                BusinessScale = "Küçük işletme"
            },
            
            // Kişisel Gelişim: Dijital Öğrenme
            new Reward
            {
                Id = "10",
                MerchantName = "Akademetre",
                City = "Online",
                Category = "Kişisel Gelişim",
                RewardType = RewardCategory.DijitalOgrenme,
                Title = "Farkındalık ve Mindfulness Eğitimi",
                CostPoints = 350,
                Stock = 50,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "12 haftalık yapılandırılmış farkındalık programı",
                LearningOutcome = "Stres yönetimi, duygusal denge ve şimdiki ana odaklanma becerileri kazanacaksınız",
                GrowthArea = "Bilinç & Farkındalık",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Zihinsel sağlığınıza uzun vadeli yatırım",
                BusinessScale = "Yerel Eğitim Platformu"
            },
            new Reward
            {
                Id = "11",
                MerchantName = "Kodluyoruz",
                City = "Online",
                Category = "Teknik Beceri",
                RewardType = RewardCategory.DijitalOgrenme,
                Title = "Yazılım Geliştirme Bootcamp",
                CostPoints = 500,
                Stock = 30,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "3 aylık yoğun yazılım eğitimi ve mentorluk programı",
                LearningOutcome = "Web geliştirme, veri yapıları ve proje yönetimi beceriler kazanacaksınız",
                GrowthArea = "Üretkenlik & Teknik Beceri",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Kariyer dönüşümünüz için sağlam temel",
                BusinessScale = "Sosyal Girişim"
            },
            
            // Kişisel Gelişim: Dijital Rehberler
            new Reward
            {
                Id = "12",
                MerchantName = "İyi Yaşam Akademisi",
                City = "Online",
                Category = "Yaşam Becerileri",
                RewardType = RewardCategory.DijitalRehber,
                Title = "Sürdürülebilir Yaşam E-Rehberi + Planlayıcı",
                CostPoints = 200,
                Stock = 80,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "120+ sayfa dijital rehber, haftalık planlama şablonları ve aksiyon listeleri",
                LearningOutcome = "Sıfır atık yaşam, bilinçli tüketim ve çevre dostu alışkanlıklar edineceksiniz",
                GrowthArea = "Bilinç & Sürdürülebilirlik",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Yaşam tarzınızı dönüştüren pratik bilgiler",
                BusinessScale = "Bağımsız İçerik Üreticisi"
            },
            new Reward
            {
                Id = "13",
                MerchantName = "Okuyan Toplum",
                City = "Online",
                Category = "Okuma & Düşünme",
                RewardType = RewardCategory.DijitalRehber,
                Title = "Eleştirel Düşünme ve Okuma Kılavuzu",
                CostPoints = 180,
                Stock = 70,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "E-kitap + sesli kitap + not alma şablonları + tartışma soruları",
                LearningOutcome = "Derin okuma, analitik düşünme ve bilgiyi sentezleme yetenekleri geliştireceksiniz",
                GrowthArea = "Öğrenme & Düşünme",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Bilgi çağında kendinizi güçlendirin",
                BusinessScale = "Eğitim Topluluğu"
            },
            
            // Kişisel Gelişim: Topluluk Erişimi
            new Reward
            {
                Id = "14",
                MerchantName = "Kadın Girişimci Ağı",
                City = "Online + İstanbul",
                Category = "Mentorluk",
                RewardType = RewardCategory.ToplulukErisim,
                Title = "3 Aylık Mentorluk Programı",
                CostPoints = 600,
                Stock = 20,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "Deneyimli girişimcilerden bire-bir mentorluk ve özel topluluk erişimi",
                LearningOutcome = "İş geliştirme stratejileri, ağ oluşturma ve girişimcilik mindset'i kazanacaksınız",
                GrowthArea = "Girişimcilik & Liderlik",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Hayal ettiğiniz projeyi hayata geçirin",
                BusinessScale = "Sosyal Ağ"
            },
            new Reward
            {
                Id = "15",
                MerchantName = "Sürdürülebilir Yaşam Topluluğu",
                City = "Online",
                Category = "Topluluk",
                RewardType = RewardCategory.ToplulukErisim,
                Title = "1 Yıllık Premium Üyelik",
                CostPoints = 400,
                Stock = 50,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "Aylık atölye çalışmaları, forum erişimi ve uzman sohbetleri",
                LearningOutcome = "Sıfır atık, bilinçli beslenme ve çevre dostu yaşam topluluğuyla büyüyeceksiniz",
                GrowthArea = "Bilinç & Topluluk",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Benzer değerlere sahip insanlarla bağ kurun",
                BusinessScale = "Kar Amacı Gütmeyen"
            },
            
            // Kişisel Gelişim: Üretkenlik Araçları
            new Reward
            {
                Id = "16",
                MerchantName = "Notion Türkiye",
                City = "Online",
                Category = "Dijital Araç",
                RewardType = RewardCategory.UretkenlikAraci,
                Title = "1 Yıllık Notion Pro + Türkçe Şablonlar",
                CostPoints = 450,
                Stock = 40,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "Üretkenlik uygulaması + 50+ özel Türkçe şablon paketi",
                LearningOutcome = "Proje yönetimi, not alma ve bilgi organizasyonu sistemleri oluşturacaksınız",
                GrowthArea = "Üretkenlik & Organizasyon",
                IsLocalBusiness = false,
                IsEthical = true,
                ImpactDescription = "Düşüncelerinizi ve projelerinizi merkezi bir yerde yönetin",
                BusinessScale = "Teknoloji Şirketi"
            },
            new Reward
            {
                Id = "17",
                MerchantName = "Odaklan",
                City = "Online",
                Category = "Üretkenlik",
                RewardType = RewardCategory.UretkenlikAraci,
                Title = "Derin Çalışma Rehberi + Pomodoro Pro",
                CostPoints = 250,
                Stock = 60,
                ValidUntil = new DateTime(2026, 12, 31),
                Description = "E-rehber + premium üretkenlik uygulaması (1 yıl)",
                LearningOutcome = "Dikkat yönetimi, odaklanma teknikleri ve verimli çalışma alışkanlıkları edineceksiniz",
                GrowthArea = "Üretkenlik & Odaklanma",
                IsLocalBusiness = true,
                IsEthical = true,
                ImpactDescription = "Zamanınızı en değerli varlığınız olarak yönetin",
                BusinessScale = "Yerel Uygulama Geliştiricisi"
            }
        };

        _context.Rewards.AddRange(rewards);
        _context.SaveChanges();
    }
}
