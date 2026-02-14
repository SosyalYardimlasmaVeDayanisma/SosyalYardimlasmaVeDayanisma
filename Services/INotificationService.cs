using SosyalYardim.Models;

namespace SosyalYardim.Services;

public interface INotificationService
{
    Task<List<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false);
    Task<int> GetUnreadCountAsync(string userId);
    Task<Notification?> GetNotificationByIdAsync(string notificationId);
    Task CreateNotificationAsync(Notification notification);
    Task MarkAsReadAsync(string notificationId);
    Task MarkAllAsReadAsync(string userId);
    Task DeleteNotificationAsync(string notificationId);
    
    // Otomatik bildirim oluşturma yardımcı metodları
    Task NotifyDonationMadeAsync(string userId, string campaignId, decimal amount, int points);
    Task NotifyPointsConvertedAsync(string userId, string rewardId, int pointsSpent);
    Task NotifyCampaignCompletedAsync(string userId, string campaignId, string campaignTitle);
    Task NotifyImpactRealizedAsync(string userId, string entityId, string impactDescription);
}
