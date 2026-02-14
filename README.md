# 🤝 SosyalYardımlaşmaVeDayanışma

Bağış ve gönüllülük süreçlerini dijitalleştiren, şeffaf ve kullanıcı odaklı bir sosyal sorumluluk platformu.

## 📑 İçindekiler

- [Platform Hakkında](#-platform-hakkında)
- [Temel Özellikler](#-temel-özellikler)
- [Teknoloji Altyapısı](#-teknoloji-altyapısı)
- [Kurulum](#-kurulum)
- [Kullanım Senaryoları](#-kullanım-senaryoları)
- [Proje Yapısı](#-proje-yapısı)
- [Katkıda Bulunma](#-katkıda-bulunma)
- [İletişim](#-iletişim)

## 📖 Platform Hakkında

SosyalYardımlaşmaVeDayanışma, toplumsal yardımlaşmayı dijital bir ekosistem içinde organize eden bir web platformudur. Platform üzerinde bireyler ve kurumlar, çeşitli sosyal sorumluluk projelerine maddi veya manevi katkı sağlayabilir. Sistem, her katkının karşılığında kullanıcılara puan kazandırır ve bu puanlar eğitim içeriklerinden ürün indirimlerine kadar farklı alanlarda kullanılabilir.

Platformun temel amacı, yardımlaşma süreçlerini basitleştirmek ve her aşamayı şeffaf hale getirmektir. Kullanıcılar yaptıkları katkıların nereye gittiğini, hangi projelere destek olduğunu ve toplam etkilerini detaylı raporlarla takip edebilir.

## ✨ Temel Özellikler

### 💳 Bağış Yönetimi

Platform, kampanya bazlı bağış sistemini üç aşamalı bir süreçle yönetir:

1. Kullanıcı, kategorilere göre filtrelenmiş kampanya listesinden destek vermek istediği projeyi seçer
2. Bağış tutarını belirler ve ödeme işlemini tamamlar
3. Sistem otomatik olarak iyilik puanı hesaplayıp kullanıcı hesabına tanımlar

Her kampanya için hedef tutar, şu ana kadar toplanan miktar ve kalan süre gibi bilgiler gerçek zamanlı olarak güncellenir. Kampanyalar sekiz farklı kategoride organize edilmiştir:

- 🆘 **Afet & Acil Durum**
- 📚 **Eğitim & Çocuk**
- 🏥 **Sağlık**
- 🐾 **Hayvanlar**
- 🍞 **Gıda & İhtiyaç**
- 🌱 **Çevre & Doğa**
- ♿ **Engelli/Yaşlı Destek**
- 🤲 **Genel Yardım**

### 🏆 İyilik Puan Sistemi

Her 10 TL katkı karşılığında kullanıcılara 1 iyilik puanı tanımlanır. Bu puanlar iki farklı şekilde kullanılabilir:

**🎓 Kişisel Gelişim**: Eğitim platformlarına üyelik, dijital kurslar, e-kitaplar, mentorluk programları gibi içeriklere erişim

**🔄 Yeniden Bağış**: Kazanılan puanları başka kampanyalara aktarma ve döngüsel katkı sağlama

Destekçi işletmeler, sürdürülebilirlik ve etik değerler çerçevesinde seçilir. Platform, işletmelerin sadece indirim sağlamadığı, aynı zamanda iyilik ekosistemine dahil olduğu bir model benimser.

### 🙋 Gönüllülük Modülü

Maddi katkının yanı sıra zaman ve bilgi paylaşımı yoluyla destek olmak isteyenler için dört farklı gönüllülük kategorisi bulunur:

- 💻 **Dijital Gönüllülük**: Web geliştirme, tasarım, içerik üretimi, sosyal medya yönetimi
- 📖 **Eğitim Desteği**: Öğrencilere mentorluk, ders desteği, okuma yazma eğitimi
- 🏃 **Saha Desteği**: Afet bölgelerinde veya kampanya organizasyonlarında fiziksel destek
- 📢 **Farkındalık Çalışmaları**: Platformu tanıtma, topluluk oluşturma, kampanyaları yaygınlaştırma

Gönüllü başvuruları form üzerinden alınır ve yönetim paneli üzerinden değerlendirilir. Onaylanan gönüllüler, ilgili kampanyalarla eşleştirilir.

### 👤 Kullanıcı Paneli

Her kullanıcının kişisel panelinde şu bilgiler yer alır:

- 📊 Toplam bağış sayısı ve tutarı
- ⭐ Kazanılan ve harcanan iyilik puanları
- 📝 Bağış geçmişi (tarih, kampanya, tutar, kazanılan puan)
- 🎯 Puan kullanım geçmişi
- 📂 Desteklenen kampanya kategorileri

Panel üzerinden kullanıcılar, katkılarının özet raporunu PDF formatında indirebilir veya sosyal medyada paylaşılabilir bir etki kartı oluşturabilir.

### 🔍 Şeffaflık Mekanizması

Platform, tüm bağış ve harcama verilerini açık bir şekilde raporlar. Şeffaflık sayfasında şu bilgiler bulunur:

- 💰 Toplam yardım miktarı
- 📋 Aktif kampanya sayısı
- 👥 Toplam katılımcı sayısı
- 💵 Ortalama katkı tutarı
- 📊 Kategorilere göre bağış dağılımı (grafik)
- 🗺️ Türkiye haritası üzerinde kampanya konumları

Sistem, hiçbir gizli kesinti veya komisyon almaz. Tüm mali akış, kampanya bazında izlenebilir.

### ⚙️ Yönetim Paneli

Platform yöneticileri için geliştirilmiş kontrol paneli şu modülleri içerir:

- 📋 **Kampanya Yönetimi**: Yeni kampanya oluşturma, mevcut kampanyaları düzenleme veya sonlandırma
- 🎁 **Ödül Yönetimi**: İyilik puanlarıyla alınabilecek ödüllerin tanımlanması, puan değerlerinin belirlenmesi
- 🏢 **İşletme Yönetimi**: Destekçi işletmelerin platforma eklenmesi, ödül anlaşmalarının yönetimi
- 👨‍👩‍👧‍👦 **Gönüllü Yönetimi**: Başvuruların incelenmesi, onaylama/reddetme işlemleri

Yönetim paneli, tüm süreçlerin merkezi bir noktadan kontrol edilmesini sağlar.

## 🛠 Teknoloji Altyapısı

### Backend Teknolojileri

- **ASP.NET Core**: Web uygulaması framework'ü olarak kullanılmaktadır. MVC mimarisi benimsenmiştir.
- **Entity Framework Core**: Veritabanı işlemleri için ORM katmanı olarak tercih edilmiştir.
- **SQL Server**: İlişkisel veritabanı yönetim sistemi olarak kullanılmaktadır.
- **ASP.NET Core Identity**: Kullanıcı kimlik doğrulama ve yetkilendirme için entegre edilmiştir.

### Frontend Teknolojileri

- **HTML5/CSS3**: Sayfa yapısı ve stil tanımlamaları
- **JavaScript**: İstemci tarafı etkileşimler
- **Bootstrap 5**: Responsive tasarım framework'ü
- **Chart.js**: Veri görselleştirme için grafik kütüphanesi
- **Leaflet.js**: Harita görselleştirmesi

### Güvenlik ve Ödeme

- 🔒 SSL sertifikası ile şifreli veri iletimi
- 💳 Güvenli ödeme gateway entegrasyonu
- 🛡️ GDPR uyumlu veri saklama politikaları

## 🚀 Kurulum

### Gereksinimler

Projeyi çalıştırmak için sisteminizde şu yazılımların yüklü olması gerekmektedir:

- .NET 6.0 SDK veya üzeri
- SQL Server 2019 veya üzeri
- Git

### Adım Adım Kurulum

**1.** Repository'yi bilgisayarınıza klonlayın:
```bash
git clone https://github.com/SosyalYardimlasmaVeDayanisma/SosyalYardimlasmaVeDayanisma.git
cd SosyalYardimlasmaVeDayanisma
```

**2.** Proje klasöründe `appsettings.json` dosyasını açın ve veritabanı bağlantı ayarlarını kendi SQL Server bilgilerinize göre düzenleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SUNUCU_ADI;Database=SosyalDayanisma;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

**3.** Veritabanı migration işlemlerini çalıştırın:
```bash
dotnet ef database update
```

Bu komut, gerekli tabloları ve ilişkileri otomatik olarak oluşturacaktır.

**4.** Projeyi başlatın:
```bash
dotnet run
```

Tarayıcınızda `https://localhost:5001` adresine giderek platforma erişebilirsiniz.

### 📌 İlk Çalıştırma Notları

İlk çalıştırmada sistemde örnek veriler bulunmayacaktır. Admin paneline giriş yaparak kampanya, ödül ve işletme tanımlamalarını yapabilirsiniz. Seed data oluşturmak isterseniz, `Data/DbInitializer.cs` dosyasını kullanabilirsiniz.

## 💡 Kullanım Senaryoları

### Senaryo 1: Bireysel Kullanıcı Bağış Süreci

1. Kullanıcı platforma kayıt olur ve giriş yapar
2. Ana sayfada kategorilere göre kampanyaları inceler
3. İlgisini çeken bir kampanyaya tıklar ve detayları okur
4. "Destek Ol" butonuna tıklayarak bağış sürecini başlatır
5. Bağış miktarını belirler (örneğin 500 TL)
6. Ödeme bilgilerini girer ve işlemi tamamlar
7. Sistem 50 iyilik puanı tanımlar ve kullanıcıya bildirim gönderir
8. Kullanıcı panelinde katkısını görüntüler

### Senaryo 2: Gönüllü Başvurusu

1. Kullanıcı "Gönüllü Ol" sayfasına gider
2. Dört gönüllülük kategorisini inceler
3. "Eğitim Desteği" kategorisini seçer
4. Başvuru formunu doldurur (ad, iletişim, il, deneyim notları)
5. Başvuruyu gönderir
6. Yönetici panelinden başvuru incelenir ve onaylanır
7. Kullanıcıya e-posta ile bilgilendirme yapılır
8. Gönüllü, ilgili kampanyalara yönlendirilir

### Senaryo 3: İyilik Puanı Kullanımı

1. Kullanıcı 250 iyilik puanı biriktirmiştir
2. "Katkıyı Dönüştür" sayfasına gider
3. İki seçenek görür: "Kendin İçin Geliştir" ve "Başkasının İyiliğine Dönüştür"
4. "Kendin İçin Geliştir" sekmesini seçer
5. 250 puanla alınabilecek "Derin Çalışma Rehberi + Pomodoro Pro" kitabını seçer
6. Puanları kullanarak indirme bağlantısını alır
7. Kalan puanı (0 puan) panelinde görüntüler

### Senaryo 4: Yönetici Kampanya Ekleme

1. Yönetici admin paneline giriş yapar
2. "Kampanyalar" modülüne tıklar
3. "Yeni Kampanya Ekle" butonuna basar
4. Kampanya bilgilerini doldurur:
   - Başlık: "Bursa'da Engelli Çocuklar İçin Özel Eğitim Merkezi"
   - Kategori: Engelli/Yaşlı Destek
   - Şehir: Bursa
   - Hedef tutar: 350.000 TL
   - Açıklama ve görseller
5. Kampanyayı aktif olarak işaretler ve kaydeder
6. Kampanya anında platformda yayınlanır
7. Kullanıcılar kampanyayı görüntüleyebilir ve destek olabilir

## 📁 Proje Yapısı
```
SosyalYardimlasmaVeDayanisma/
│
├── Controllers/
│   ├── HomeController.cs          # Ana sayfa ve genel işlemler
│   ├── CampaignController.cs      # Kampanya listeleme ve detay
│   ├── DonationController.cs      # Bağış süreçleri
│   ├── UserController.cs          # Kullanıcı paneli
│   ├── VolunteerController.cs     # Gönüllülük başvuruları
│   └── AdminController.cs         # Yönetim paneli
│
├── Models/
│   ├── Campaign.cs                # Kampanya modeli
│   ├── Donation.cs                # Bağış modeli
│   ├── User.cs                    # Kullanıcı modeli
│   ├── GoodPoint.cs               # İyilik puanı modeli
│   ├── Reward.cs                  # Ödül modeli
│   ├── Partner.cs                 # İşletme ortağı modeli
│   └── Volunteer.cs               # Gönüllü başvuru modeli
│
├── Views/
│   ├── Home/                      # Ana sayfa görünümleri
│   ├── Campaign/                  # Kampanya görünümleri
│   ├── Donation/                  # Bağış süreci görünümleri
│   ├── User/                      # Kullanıcı paneli görünümleri
│   ├── Volunteer/                 # Gönüllülük görünümleri
│   ├── Admin/                     # Yönetim paneli görünümleri
│   └── Shared/                    # Paylaşılan layout ve componentler
│
├── Data/
│   ├── ApplicationDbContext.cs    # EF Core context
│   └── DbInitializer.cs          # Seed data
│
├── Migrations/                    # Veritabanı migration dosyaları
│
├── Services/
│   ├── DonationService.cs        # Bağış iş mantığı
│   ├── PointService.cs           # Puan hesaplama servisi
│   └── NotificationService.cs    # Bildirim servisi
│
├── wwwroot/
│   ├── css/                      # Stil dosyaları
│   ├── js/                       # JavaScript dosyaları
│   ├── images/                   # Görseller
│   └── lib/                      # Üçüncü parti kütüphaneler
│
├── appsettings.json              # Konfigürasyon
├── Program.cs                    # Uygulama giriş noktası
└── Startup.cs                    # Servis konfigürasyonları
```

## 🤝 Katkıda Bulunma

Projeye katkı sağlamak isteyenler için geliştirme süreci şu şekilde işler:

1. Repository'yi fork edin
2. Yeni bir branch oluşturun (`git checkout -b yeni-ozellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik: Kullanıcı profil fotoğrafı'`)
4. Branch'inizi push edin (`git push origin yeni-ozellik`)
5. Pull Request açın

### 📋 Kod yazarken dikkat edilmesi gerekenler:

- ✅ C# coding conventions'larına uygun yazın
- 💬 Karmaşık fonksiyonlara yorum satırları ekleyin
- 🏷️ Değişkenlere açıklayıcı isimler verin
- 🎯 Mümkün olduğunca SOLID prensiplerini uygulayın

## 🗓️ Gelecek Planlar

Platform için planladığımız geliştirmeler:

- 📱 **Mobil Uygulama**: iOS ve Android için native uygulamalar
- 🏢 **Kurumsal Panel**: Şirketlerin çalışan gönüllülüğünü yönetebileceği ayrı bir panel
- 💬 **SMS Bildirimleri**: Kampanya güncellemeleri için SMS entegrasyonu
- 🌍 **Çoklu Dil Desteği**: İngilizce, Almanca ve Arapça dil seçenekleri
- 🔌 **API Geliştirme**: Üçüncü parti entegrasyonlar için RESTful API
- ⛓️ **Blockchain Entegrasyonu**: Bağışların takibi için blockchain kayıt sistemi

## 📊 İstatistikler

Platform üzerinde güncel durumu yansıtan bazı veriler:

| Metrik | Değer |
|--------|-------|
| 📋 Aktif kampanya sayısı | 6 |
| 💰 Toplam toplanan bağış | 906.600 TL |
| 👥 Kayıtlı kullanıcı sayısı | 15.234 |
| 💵 Ortalama bağış tutarı | 623 TL |
| 🏢 Destekçi işletme sayısı | 9 |

## 📞 İletişim

Proje hakkında soru, öneri veya işbirliği teklifleriniz için:

📧 **E-posta**: albayrak01asiye@gmail.com  
👤 **Geliştirici**: Elif Asiye ALBAYRAK  
🌐 **Website**: sosyaldayanisma.com

## 📄 Lisans

Bu proje MIT lisansı altında sunulmaktadır. Detaylı bilgi için `LICENSE` dosyasına bakabilirsiniz.

---

<div align="center">

💚 **Platform, ticari amaç gütmeden sosyal fayda odaklı olarak geliştirilmiştir.**  
**Tüm katkılar kampanyalara yansıtılır, platformdan herhangi bir kesinti yapılmaz.**

[⬆ Başa Dön](#-sosyal-dayanışma-ağı)

</div>
