using SKANSAPUNG.API.Models;

namespace SKANSAPUNG.API.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationRequest notificationRequest);
    }
} 