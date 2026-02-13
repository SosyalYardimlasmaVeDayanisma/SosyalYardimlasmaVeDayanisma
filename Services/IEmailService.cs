namespace AfetPuan.Services;

public interface IEmailService
{
    Task SendDonationConfirmationEmailAsync(string recipientEmail, string recipientName, string campaignTitle, decimal amount, int pointsEarned);
    Task SendVolunteerApplicationStatusEmailAsync(string recipientEmail, string recipientName, string status, string? adminNotes);
}
