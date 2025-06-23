using SKANSAPUNG.MAUI.Views;

namespace SKANSAPUNG.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(StudentsPage), typeof(StudentsPage));
        Routing.RegisterRoute(nameof(AttendancePage), typeof(AttendancePage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(StudentDetailPage), typeof(StudentDetailPage));
        Routing.RegisterRoute(nameof(ExtracurricularPage), typeof(ExtracurricularPage));
        Routing.RegisterRoute(nameof(TeachersPage), typeof(TeachersPage));
        Routing.RegisterRoute(nameof(ClassesPage), typeof(ClassesPage));
        Routing.RegisterRoute(nameof(ReportsPage), typeof(ReportsPage));
        Routing.RegisterRoute(nameof(GradesPage), typeof(GradesPage));
        Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
        Routing.RegisterRoute(nameof(MyGradesPage), typeof(MyGradesPage));
        Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
    }
} 