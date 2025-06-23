using Microsoft.Maui.Devices;
using SKANSAPUNG.MAUI.Models;
using System.Diagnostics;
#if FIREBASE
using Plugin.Firebase.CloudMessaging;
#endif

namespace SKANSAPUNG.MAUI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IApiService _apiService;

        public NotificationService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> GetTokenAsync()
        {
#if FIREBASE
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
            Debug.WriteLine($"FCM token: {token}");
            return token;
#else
            return await Task.FromResult("dummy_token_not_firebase");
#endif
        }

        public async Task SendNotificationAsync(string title, string body, string token)
        {
            // Implementation for sending notifications via backend API
            var notification = new NotificationRequest
            {
                To = token,
                Title = title,
                Body = body
            };

            await _apiService.SendNotificationAsync(notification);
        }

        public async Task RegisterDeviceAsync()
        {
            try
            {
                var token = await GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    var deviceId = DeviceInfo.Current.Idiom.ToString(); // Idiom (tablet/phone)
                    var platform = DeviceInfo.Current.Platform.ToString();
                    
                    await _apiService.RegisterFcmTokenAsync(token, deviceId, platform);
                    Debug.WriteLine("Device token registered with backend.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error registering device token: {ex.Message}");
            }
        }
    }
} 