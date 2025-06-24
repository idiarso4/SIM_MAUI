namespace SKANSAPUNG.MAUI;
using System.Diagnostics;
#if FIREBASE
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.DynamicLinks;
#endif
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;

public partial class App : Application
{
    public App(AppShell appShell)
    {
        InitializeComponent();

        MainPage = appShell;

        SubscribeToNotificationEvents();
    }

    private void SubscribeToNotificationEvents()
    {
#if FIREBASE
        CrossFirebaseCloudMessaging.Current.NotificationReceived += (sender, e) =>
        {
            System.Diagnostics.Debug.WriteLine("Notification received");

            var notification = new LocalNotification
            {
                Title = e.Notification.Title,
                Body = e.Notification.Body,
                ReceivedAt = DateTime.Now,
                IsRead = false
            };

            // Simpan notifikasi ke database lokal
            var dbService = IPlatformApplication.Current.Services.GetService<IDatabaseService>();
            dbService?.SaveNotificationAsync(notification);

            // Tampilkan notifikasi sebagai alert saat aplikasi di foreground
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Current.MainPage.DisplayAlert(e.Notification.Title, e.Notification.Body, "OK");
            });
        };

        CrossFirebaseCloudMessaging.Current.NotificationTapped += (sender, e) =>
        {
            System.Diagnostics.Debug.WriteLine("Notification tapped");

            // TODO: Tambahkan navigasi ke halaman spesifik jika perlu
            // Contoh: Navigasi ke halaman notifikasi
            // Shell.Current.GoToAsync("//NotificationsPage");
        };
#endif
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }
} 