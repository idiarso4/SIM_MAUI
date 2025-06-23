using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class NotificationsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;
        private readonly IDatabaseService _databaseService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private ObservableCollection<LocalNotification> _notifications;

        [ObservableProperty]
        private LocalNotification _selectedNotification;

        public NotificationsViewModel(IApiService apiService, IAuthService authService, IDatabaseService databaseService, 
            INotificationService notificationService, IConnectivityService connectivityService)
            : base(connectivityService)
        {
            _apiService = apiService;
            _authService = authService;
            _databaseService = databaseService;
            _notificationService = notificationService;
            Title = "Notifications";
            Notifications = new ObservableCollection<LocalNotification>();
        }

        [RelayCommand]
        private async Task LoadNotificationsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    await ShowAlertAsync("Error", "You must be logged in to view notifications.");
                    return;
                }

                if (ConnectivityService.IsConnected())
                {
                    // Online: Get from API and save to local DB
                    var apiNotifications = await _apiService.GetNotificationsByUserAsync(currentUser.Id);
                    await _databaseService.SaveNotificationsAsync(apiNotifications.Select(n => new LocalNotification
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        CreatedAt = n.CreatedAt,
                        IsRead = n.IsRead,
                        UserId = n.UserId
                    }).ToList());
                }

                // Always load from local DB for UI
                var localNotifications = await _databaseService.GetNotificationsAsync(currentUser.Id);
                Notifications.Clear();
                foreach (var notification in localNotifications)
                {
                    Notifications.Add(notification);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading notifications: {ex.Message}");
                await ShowAlertAsync("Error", "Failed to load notifications.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task MarkAsReadAsync(LocalNotification notification)
        {
            if (notification == null) return;

            try
            {
                notification.IsRead = true;
                await _databaseService.UpdateNotificationAsync(notification);

                if (ConnectivityService.IsConnected())
                {
                    await _apiService.MarkNotificationAsReadAsync(notification.Id);
                }

                // Refresh the list
                var index = Notifications.IndexOf(notification);
                if (index >= 0)
                {
                    Notifications[index] = notification;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error marking notification as read: {ex.Message}");
                await ShowAlertAsync("Error", "Failed to mark notification as read.");
            }
        }

        [RelayCommand]
        private async Task RegisterForPushNotificationsAsync()
        {
            try
            {
                var token = await _notificationService.RegisterForPushNotificationsAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    var currentUser = await _authService.GetCurrentUserAsync();
                    if (currentUser != null && ConnectivityService.IsConnected())
                    {
                        await _apiService.RegisterTokenAsync(token, DeviceInfo.Current.Model);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error registering for push notifications: {ex.Message}");
                await ShowAlertAsync("Error", "Failed to register for push notifications.");
            }
        }

        protected override async Task OnRefreshAsync()
        {
            await LoadNotificationsAsync();
        }
    }
} 