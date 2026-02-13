using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AfetPuan.Services;
using System.Security.Claims;

namespace AfetPuan.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return View(notifications);
    }

    [HttpGet]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Json(new { count = 0 });

        var count = await _notificationService.GetUnreadCountAsync(userId);
        return Json(new { count });
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        await _notificationService.MarkAllAsReadAsync(userId);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _notificationService.DeleteNotificationAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
