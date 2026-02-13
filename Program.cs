using AfetPuan.Services;
using AfetPuan.Data;
using AfetPuan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/Login";
});

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<ITransparencyService, MockTransparencyService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IImpactVerificationService, ImpactVerificationService>();
builder.Services.AddScoped<IImpactCardService, ImpactCardService>();
builder.Services.AddScoped<IPdfExportService, PdfExportService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        
        context.Database.EnsureCreated();
        
        // Rolleri oluştur
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
        
        // Admin kullanıcısı oluştur
        var adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Admin",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Admin kullanıcısı varsa rolünü kontrol et
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed data: Demo kampanyaları oluştur (öne çıkan örnekler)
        if (!context.Campaigns.Any())
        {
            var demoCampaigns = new List<Campaign>
            {
                new Campaign
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Kahramanmaraş Deprem Bölgesi Acil Yardım",
                    Description = "6 Şubat depremlerinden etkilenen Kahramanmaraş'taki aileler için acil ihtiyaç paketleri hazırlıyoruz. Gıda, temizlik malzemeleri, battaniye ve temel ihtiyaç ürünleri içeren paketler dağıtılacak.",
                    Category = CampaignCategory.AfetAcilDurum,
                    DisasterType = DisasterType.Deprem,
                    City = "Kahramanmaraş",
                    GoalAmount = 500000,
                    RaisedAmount = 342750,
                    Status = CampaignStatus.Aktif,
                    NgoName = "Kızılay",
                    CreatedAt = DateTime.Now.AddDays(-15),
                    CoverUrl = "https://images.unsplash.com/photo-1488521787991-ed7bbaae773c?w=800",
                    IsFeatured = true,
                    Priority = 100
                },
                new Campaign
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "İstanbul Okul Öncesi Eğitim Desteği",
                    Description = "İstanbul'un dezavantajlı bölgelerinde yaşayan çocuklar için okul öncesi eğitim materyalleri, kırtasiye ve eğitim setleri sağlıyoruz. Her çocuğun eğitime eşit erişim hakkı vardır.",
                    Category = CampaignCategory.EgitimCocuk,
                    DisasterType = DisasterType.Diğer,
                    City = "İstanbul",
                    GoalAmount = 150000,
                    RaisedAmount = 98500,
                    Status = CampaignStatus.Aktif,
                    NgoName = "UNICEF Türkiye",
                    CreatedAt = DateTime.Now.AddDays(-8),
                    CoverUrl = "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?w=800",
                    IsFeatured = true,
                    Priority = 90
                },
                new Campaign
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "İzmir Sokak Hayvanları Bakım Merkezi",
                    Description = "İzmir'de sokak hayvanları için modern bir bakım ve tedavi merkezi kuruyoruz. Veteriner hizmetleri, aşılama, kısırlaştırma ve barınma imkanları sağlanacak.",
                    Category = CampaignCategory.Hayvanlar,
                    DisasterType = DisasterType.Diğer,
                    City = "İzmir",
                    GoalAmount = 250000,
                    RaisedAmount = 127800,
                    Status = CampaignStatus.Aktif,
                    NgoName = "Haytap",
                    CreatedAt = DateTime.Now.AddDays(-20),
                    CoverUrl = "https://images.unsplash.com/photo-1450778869180-41d0601e046e?w=800",
                    IsFeatured = true,
                    Priority = 85
                },
                new Campaign
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Ankara Yaşlı Bakım ve Dayanışma Evi",
                    Description = "Ankara'da yalnız yaşayan ve bakıma muhtaç yaşlı vatandaşlarımız için kapsamlı bakım hizmetleri ve sosyal destek programları sunuyoruz.",
                    Category = CampaignCategory.YasliDestegi,
                    DisasterType = DisasterType.Diğer,
                    City = "Ankara",
                    GoalAmount = 200000,
                    RaisedAmount = 87500,
                    Status = CampaignStatus.Aktif,
                    NgoName = "Yaşlılara Saygı Derneği",
                    CreatedAt = DateTime.Now.AddDays(-12),
                    CoverUrl = "https://images.unsplash.com/photo-1581579438747-1dc8d17bbce4?w=800",
                    IsFeatured = true,
                    Priority = 80
                },
                new Campaign
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Engelli Çocuklar İçin Özel Eğitim Merkezi",
                    Description = "Bursa'da özel eğitim ihtiyacı olan çocuklarımız için modern terapi odaları, özel eğitim araçları ve uzman kadro ile donatılmış bir merkez kuruyoruz.",
                    Category = CampaignCategory.EngelliDogumluBireyler,
                    DisasterType = DisasterType.Diğer,
                    City = "Bursa",
                    GoalAmount = 350000,
                    RaisedAmount = 156200,
                    Status = CampaignStatus.Aktif,
                    NgoName = "Engelsiz Yaşam Vakfı",
                    CreatedAt = DateTime.Now.AddDays(-18),
                    CoverUrl = "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?w=800",
                    IsFeatured = true,
                    Priority = 75
                },
                new Campaign
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Antalya Çevre Koruma ve Ağaçlandırma",
                    Description = "Antalya ormanlarında yangınlardan zarar görmüş alanların yeniden ağaçlandırılması ve çevre koruma farkındalık eğitimleri düzenliyoruz.",
                    Category = CampaignCategory.CevreVeDoga,
                    DisasterType = DisasterType.Yangın,
                    City = "Antalya",
                    GoalAmount = 180000,
                    RaisedAmount = 92300,
                    Status = CampaignStatus.Aktif,
                    NgoName = "Yeşil Dünya Derneği",
                    CreatedAt = DateTime.Now.AddDays(-25),
                    CoverUrl = "https://images.unsplash.com/photo-1542601906990-b4d3fb778b09?w=800",
                    IsFeatured = true,
                    Priority = 70
                }
            };

            context.Campaigns.AddRange(demoCampaigns);
            await context.SaveChangesAsync();

            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"✅ {demoCampaigns.Count} adet öne çıkan demo kampanya oluşturuldu");
        }
        
        // Seed data: CampaignService ve RewardService için
        var campaignService = services.GetRequiredService<ICampaignService>();
        var rewardService = services.GetRequiredService<IRewardService>();
        
        if (campaignService is CampaignService cs)
        {
            cs.InitializeSampleData();
        }
        
        if (rewardService is RewardService rs)
        {
            rs.InitializeSampleData();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı başlatma hatası");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

