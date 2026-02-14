using SosyalYardim.Models;
using SosyalYardim.Data;
using Microsoft.EntityFrameworkCore;

namespace SosyalYardim.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId);
        
        if (unreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }
        
        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(50) // Son 50 bildirim
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .CountAsync();
    }

    public async Task<Notification?> GetNotificationByIdAsync(string notificationId)
    {
        return await _context.Notifications.FindAsync(notificationId);
    }

    public async Task CreateNotificationAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(string notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();
        
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(string notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }

    // Otomatik bildirim oluşturma yardımcı metodları
    public async Task NotifyDonationMadeAsync(string userId, string campaignId, decimal amount, int points)
    {
        var campaign = await _context.Campaigns.FindAsync(campaignId);
        var notification = new Notification
        {
            UserId = userId,
            Type = NotificationType.DonationMade,
            Title = "Katkınız Alındı!",
            Message = $"{campaign?.Title ?? "Kampanya"}ya {amount:C} katkı yaptınız ve {points} puan kazandınız.",
            RelatedEntityId = campaignId,
            RelatedEntityType = "Campaign"
        };
        
        await CreateNotificationAsync(notification);
    }

    public async Task NotifyPointsConvertedAsync(string userId, string rewardId, int pointsSpent)
    {
        var reward = await _context.Rewards.FindAsync(rewardId);
        var notification = new Notification
        {
            UserId = userId,
            Type = NotificationType.PointsConverted,
            Title = "Puanlarınız Kullanıldı",
            Message = $"{pointsSpent} puan kullanarak '{reward?.Title ?? "Ödül"}' kazandınız.",
            RelatedEntityId = rewardId,
            RelatedEntityType = "Reward"
        };
        
        await CreateNotificationAsync(notification);
    }

    public async Task NotifyCampaignCompletedAsync(string userId, string campaignId, string campaignTitle)
    {
        var notification = new Notification
        {
            UserId = userId,
            Type = NotificationType.CampaignCompleted,
            Title = "Kampanya Tamamlandı!",
            Message = $"Desteklediğiniz '{campaignTitle}' kampanyası hedefine ulaştı. Teşekkürler!",
            RelatedEntityId = campaignId,
            RelatedEntityType = "Campaign"
        };
        
        await CreateNotificationAsync(notification);
    }

    public async Task NotifyImpactRealizedAsync(string userId, string entityId, string impactDescription)
    {
        var notification = new Notification
        {
            UserId = userId,
            Type = NotificationType.ImpactRealized,
            Title = "Etkiniz Gerçekleşti",
            Message = impactDescription,
            RelatedEntityId = entityId,
            RelatedEntityType = "Impact"
        };
        
        await CreateNotificationAsync(notification);
    }
}
