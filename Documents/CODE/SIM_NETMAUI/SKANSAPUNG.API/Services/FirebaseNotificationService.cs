using FirebaseAdmin.Messaging;
using SKANSAPUNG.API.Models;

namespace SKANSAPUNG.API.Services
{
    public class FirebaseNotificationService : INotificationService
    {
        public async Task SendNotificationAsync(NotificationRequest notificationRequest)
        {
            var message = new Message()
            {
                Token = notificationRequest.DeviceToken,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notificationRequest.Title,
                    Body = notificationRequest.Body
                },
                Data = notificationRequest.Data
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
} 