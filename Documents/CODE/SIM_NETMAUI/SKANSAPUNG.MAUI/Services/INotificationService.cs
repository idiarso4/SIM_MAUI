namespace SKANSAPUNG.MAUI.Services
{
    public interface INotificationService
    {
        Task<string> GetTokenAsync();
        Task SendNotificationAsync(string title, string body, string token);
        Task RegisterForPushNotificationsAsync();
        Task UnregisterFromPushNotificationsAsync();
        Task<bool> CheckNotificationPermissionAsync();
        Task<bool> RequestNotificationPermissionAsync();
        Task RegisterDeviceAsync();
    }
} 