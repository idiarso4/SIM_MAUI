using Microsoft.Extensions.Logging;
using SKANSAPUNG.MAUI.Services;
using SKANSAPUNG.MAUI.ViewModels;
using SKANSAPUNG.MAUI.Views;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using SKANSAPUNG.MAUI.Models.Configuration;

namespace SKANSAPUNG.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
                    var builder = MauiApp.CreateBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("SKANSAPUNG.MAUI.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            builder.Services.AddSingleton(apiSettings);
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IGeolocationService, GeolocationService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();

        // Register ViewModels
        builder.Services.AddSingleton<ClassRoomViewModel>();
        builder.Services.AddSingleton<DepartmentViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<ExtracurricularViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<StudentsViewModel>();
        builder.Services.AddSingleton<StudentDetailViewModel>();
        builder.Services.AddSingleton<StudentScoreViewModel>();
        builder.Services.AddSingleton<AssessmentViewModel>();
        builder.Services.AddSingleton<TeachersViewModel>();
        builder.Services.AddSingleton<SubjectsViewModel>();
        builder.Services.AddSingleton<ReportsViewModel>();
        builder.Services.AddSingleton<GradesViewModel>();
        builder.Services.AddSingleton<ScheduleViewModel>();
        builder.Services.AddSingleton<MyGradesViewModel>();

        // Register Views
        builder.Services.AddTransient<AttendancePage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<ExtracurricularPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<StudentDetailPage>();
        builder.Services.AddTransient<StudentsPage>();
        builder.Services.AddTransient<TeachersPage>();
        builder.Services.AddTransient<SubjectsPage>();
        builder.Services.AddTransient<DepartmentPage>();
        builder.Services.AddTransient<ClassesPage>();
        builder.Services.AddTransient<ReportsPage>();
        builder.Services.AddTransient<GradesPage>();
        builder.Services.AddTransient<SchedulePage>();
        builder.Services.AddTransient<MyGradesPage>();

        builder.Services.AddTransient<NotificationsViewModel>();
        builder.Services.AddTransient<NotificationsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
} 