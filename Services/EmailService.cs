using System.Net;
using System.Net.Mail;

namespace AfetPuan.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendDonationConfirmationEmailAsync(string recipientEmail, string recipientName, string campaignTitle, decimal amount, int pointsEarned)
    {
        try
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var senderEmail = _configuration["Email:SenderEmail"] ?? "noreply@iyilikpuan.com";
            var senderName = _configuration["Email:SenderName"] ?? "İyilik Puan";
            var senderPassword = _configuration["Email:SenderPassword"];

            // Eğer SMTP ayarları yoksa, sadece log'a yaz (geliştirme ortamı için)
            if (string.IsNullOrEmpty(senderPassword))
            {
                _logger.LogWarning("SMTP ayarları yapılandırılmamış. E-posta gönderimi simüle ediliyor.");
                _logger.LogInformation($"[E-POSTA SİMÜLASYONU] Alıcı: {recipientEmail} | Kampanya: {campaignTitle} | Tutar: ₺{amount} | Puan: {pointsEarned}");
                return;
            }

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = "Bağışınız İçin Teşekkürler! 💚",
                Body = CreateDonationEmailBody(recipientName, campaignTitle, amount, pointsEarned),
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipientEmail);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Bağış onay e-postası gönderildi: {recipientEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"E-posta gönderilirken hata oluştu: {recipientEmail}");
            // E-posta gönderimi başarısız olsa bile uygulama çalışmaya devam etsin
        }
    }

    public async Task SendVolunteerApplicationStatusEmailAsync(string recipientEmail, string recipientName, string status, string? adminNotes)
    {
        try
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var senderEmail = _configuration["Email:SenderEmail"] ?? "noreply@iyilikpuan.com";
            var senderName = _configuration["Email:SenderName"] ?? "İyilik Puan";
            var senderPassword = _configuration["Email:SenderPassword"];

            // Eğer SMTP ayarları yoksa, sadece log'a yaz (geliştirme ortamı için)
            if (string.IsNullOrEmpty(senderPassword))
            {
                _logger.LogWarning("SMTP ayarları yapılandırılmamış. E-posta gönderimi simüle ediliyor.");
                _logger.LogInformation($"[E-POSTA SİMÜLASYONU] Alıcı: {recipientEmail} | Gönüllü Başvuru Durumu: {status}");
                return;
            }

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            var subject = status == "Onaylandı" 
                ? "Gönüllülük Başvurunuz Onaylandı! 🎉" 
                : "Gönüllülük Başvurunuz Hakkında";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = CreateVolunteerStatusEmailBody(recipientName, status, adminNotes),
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipientEmail);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Gönüllü başvuru durumu e-postası gönderildi: {recipientEmail} - {status}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"E-posta gönderilirken hata oluştu: {recipientEmail}");
            // E-posta gönderimi başarısız olsa bile uygulama çalışmaya devam etsin
        }
    }

    private string CreateDonationEmailBody(string recipientName, string campaignTitle, decimal amount, int pointsEarned)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9fafb; }}
        .card {{ background: white; border-radius: 12px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .logo {{ font-size: 28px; font-weight: 900; color: #10B981; }}
        .icon {{ font-size: 48px; margin: 20px 0; }}
        h1 {{ color: #111827; font-size: 24px; margin-bottom: 10px; }}
        .amount {{ font-size: 32px; font-weight: 900; color: #10B981; margin: 20px 0; }}
        .points {{ background: linear-gradient(135deg, #10B981 0%, #059669 100%); color: white; padding: 15px; border-radius: 8px; text-align: center; margin: 20px 0; }}
        .campaign {{ background: #f3f4f6; padding: 15px; border-radius: 8px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 30px; color: #6B7280; font-size: 14px; }}
        .btn {{ display: inline-block; background: #10B981; color: white; padding: 12px 24px; border-radius: 8px; text-decoration: none; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='card'>
            <div class='header'>
                <div class='logo'>💚 İyilik Puan</div>
                <div class='icon'>✨</div>
                <h1>Bağışınız İçin Teşekkürler!</h1>
            </div>

            <p>Sayın <strong>{recipientName}</strong>,</p>
            
            <p>Yaptığınız değerli bağış için size en içten teşekkürlerimizi sunarız. Her bir katkınız, ihtiyaç sahiplerine umut oluyor.</p>

            <div class='campaign'>
                <strong>📋 Kampanya:</strong><br>
                {campaignTitle}
            </div>

            <div style='text-align: center;'>
                <div class='amount'>₺{amount:N2}</div>
                <p style='color: #6B7280;'>Bağış Tutarı</p>
            </div>

            <div class='points'>
                <div style='font-size: 24px; font-weight: 700;'>🌟 +{pointsEarned} Puan Kazandınız!</div>
                <p style='margin: 5px 0 0 0; opacity: 0.9;'>Bu puanlarla ödüllere dönüştürebilir veya başka kampanyalara katkıda bulunabilirsiniz.</p>
            </div>

            <p>Bağışınızın kullanımını ve etkisini platformumuzdan şeffaf bir şekilde takip edebilirsiniz.</p>

            <div style='text-align: center;'>
                <a href='http://localhost:5000/Profile' class='btn'>Profilime Git</a>
            </div>

            <div class='footer'>
                <p><strong>İyilik Puan Platformu</strong></p>
                <p>Her bağış bir umut, her puan bir iyilik!</p>
                <p style='font-size: 12px; color: #9CA3AF; margin-top: 20px;'>
                    Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.
                </p>
            </div>
        </div>
    </div>
</body>
</html>";
    }

    private string CreateVolunteerStatusEmailBody(string recipientName, string status, string? adminNotes)
    {
        var isApproved = status == "Onaylandı";
        var icon = isApproved ? "🎉" : "📝";
        var color = isApproved ? "#10B981" : "#F59E0B";
        var title = isApproved ? "Başvurunuz Onaylandı!" : "Başvurunuz Hakkında Bilgilendirme";
        var message = isApproved 
            ? "Gönüllülük başvurunuz değerlendirilerek onaylandı. Yakında ekibimiz sizinle iletişime geçecektir."
            : "Gönüllülük başvurunuz değerlendirildi. Aşağıdaki bilgileri inceleyebilirsiniz.";

        var adminNotesSection = !string.IsNullOrEmpty(adminNotes)
            ? $@"
            <div style='background: #f3f4f6; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid {color};'>
                <strong style='color: #111827; display: block; margin-bottom: 8px;'>Yönetici Notu:</strong>
                <p style='margin: 0; color: #4B5563;'>{adminNotes}</p>
            </div>"
            : "";

        var nextSteps = isApproved
            ? @"
            <div style='background: #EEF2FF; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                <h3 style='color: #111827; font-size: 16px; margin: 0 0 12px 0;'>📋 Sonraki Adımlar:</h3>
                <ul style='margin: 0; padding-left: 20px; color: #4B5563;'>
                    <li style='margin-bottom: 8px;'>Ekibimiz en kısa sürede sizinle iletişime geçecektir.</li>
                    <li style='margin-bottom: 8px;'>Gönüllülük faaliyetleri hakkında detaylı bilgi verilecektir.</li>
                    <li style='margin-bottom: 8px;'>Telefon ve e-posta yoluyla bilgilendirileceksiniz.</li>
                </ul>
            </div>"
            : @"
            <div style='background: #FEF3C7; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                <p style='margin: 0; color: #78350F;'>
                    Daha sonra tekrar başvuru yapabilirsiniz. İyilik ekosistemimize gösterdiğiniz ilgi için teşekkür ederiz.
                </p>
            </div>";

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9fafb; }}
        .card {{ background: white; border-radius: 12px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .logo {{ font-size: 28px; font-weight: 900; color: #10B981; }}
        .icon {{ font-size: 48px; margin: 20px 0; }}
        h1 {{ color: #111827; font-size: 24px; margin-bottom: 10px; }}
        .status-badge {{ display: inline-block; background: {color}; color: white; padding: 8px 16px; border-radius: 20px; font-weight: 600; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 30px; color: #6B7280; font-size: 14px; }}
        .btn {{ display: inline-block; background: #10B981; color: white; padding: 12px 24px; border-radius: 8px; text-decoration: none; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='card'>
            <div class='header'>
                <div class='logo'>💚 İyilik Puan</div>
                <div class='icon'>{icon}</div>
                <h1>{title}</h1>
            </div>

            <p>Sayın <strong>{recipientName}</strong>,</p>
            
            <p>{message}</p>

            <div style='text-align: center;'>
                <span class='status-badge'>{status}</span>
            </div>

            {adminNotesSection}

            {nextSteps}

            {(isApproved ? @"
            <p style='color: #6B7280; margin-top: 20px;'>
                Birlikte daha fazla iyilik yapacağımız için heyecanlıyız! 💪
            </p>" : "")}

            <div style='text-align: center;'>
                <a href='http://localhost:5000' class='btn'>Platforma Git</a>
            </div>

            <div class='footer'>
                <p><strong>İyilik Puan Platformu</strong></p>
                <p>Her bağış bir umut, her puan bir iyilik!</p>
                <p style='font-size: 12px; color: #9CA3AF; margin-top: 20px;'>
                    Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.
                </p>
            </div>
        </div>
    </div>
</body>
</html>";
    }
}
