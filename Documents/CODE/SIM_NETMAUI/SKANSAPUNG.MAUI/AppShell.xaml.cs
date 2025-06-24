using SKANSAPUNG.MAUI.Views;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;

namespace SKANSAPUNG.MAUI;

public partial class AppShell : Shell
{
    private readonly IAuthService _authService;

    public AppShell(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;

        // Register routes for pages that might not be in the initial tab bar
        RegisterRoutes();

        // Subscribe to login/logout messages
        MessagingCenter.Subscribe<object>(this, "UpdateTabs", async (sender) =>
        {
            await BuildTabsForCurrentUser();
        });

        // Initial tab setup on startup
        Task.Run(async () => await BuildTabsForCurrentUser());
    }

    private async Task BuildTabsForCurrentUser()
    {
        MainThread.BeginInvokeOnMainThread(() => MainTabBar.Items.Clear());

        var user = await _authService.GetUserAsync();
        if (user == null) return; // No user, no tabs

        // Add tabs based on user role
        switch (user.Role)
        {
            case Role.Siswa:
                AddTab("Dashboard", "dashboard.png", typeof(DashboardPage));
                AddTab("Schedule", "schedule.svg", typeof(SchedulePage));
                AddTab("My Grades", "grades.svg", typeof(MyGradesPage));
                AddTab("Profile", "profile.png", typeof(ProfilePage));
                break;

            case Role.Guru:
                AddTab("Dashboard", "dashboard.png", typeof(DashboardPage));
                AddTab("Students", "students.png", typeof(StudentsPage));
                AddTab("Attendance", "attendance.png", typeof(AttendancePage));
                AddTab("Grades", "grades.svg", typeof(GradesPage));
                AddTab("Schedule", "schedule.svg", typeof(SchedulePage));
                AddTab("Profile", "profile.png", typeof(ProfilePage));
                break;

            case Role.Admin:
            case Role.SuperAdmin:
                AddTab("Dashboard", "dashboard.png", typeof(DashboardPage));
                AddTab("Students", "students.png", typeof(StudentsPage));
                AddTab("Teachers", "teachers.svg", typeof(TeachersPage));
                AddTab("Subjects", "subjects.svg", typeof(SubjectsPage));
                AddTab("Classes", "classes.svg", typeof(ClassesPage));
                AddTab("Departments", "departments.svg", typeof(DepartmentPage));
                AddTab("Reports", "reports.svg", typeof(ReportsPage));
                AddTab("Profile", "profile.png", typeof(ProfilePage));
                break;
        }
    }

    private void AddTab(string title, string icon, Type pageType)
    {
        var tab = new Tab { Title = title, Icon = icon };
        tab.Items.Add(new ShellContent { ContentTemplate = new DataTemplate(pageType) });
        MainThread.BeginInvokeOnMainThread(() => MainTabBar.Items.Add(tab));
    }

    private void RegisterRoutes()
    {
        // Register all pages to be safe, especially for navigation from pages not in the tab bar
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(StudentsPage), typeof(StudentsPage));
        Routing.RegisterRoute(nameof(TeachersPage), typeof(TeachersPage));
        Routing.RegisterRoute(nameof(SubjectsPage), typeof(SubjectsPage));
        Routing.RegisterRoute(nameof(DepartmentPage), typeof(DepartmentPage));
        Routing.RegisterRoute(nameof(AttendancePage), typeof(AttendancePage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
        Routing.RegisterRoute(nameof(StudentDetailPage), typeof(StudentDetailPage));
        Routing.RegisterRoute(nameof(ExtracurricularPage), typeof(ExtracurricularPage));
        Routing.RegisterRoute(nameof(TeachersPage), typeof(TeachersPage));
        Routing.RegisterRoute(nameof(ClassesPage), typeof(ClassesPage));
        Routing.RegisterRoute(nameof(ReportsPage), typeof(ReportsPage));
        Routing.RegisterRoute(nameof(GradesPage), typeof(GradesPage));
        Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
        Routing.RegisterRoute(nameof(MyGradesPage), typeof(MyGradesPage));

        // Report Detail Pages
        Routing.RegisterRoute("AttendanceReportPage", typeof(AttendanceReportPage));
        Routing.RegisterRoute("GradesReportPage", typeof(GradesReportPage));
        Routing.RegisterRoute(nameof(AttendanceDetailPage), typeof(AttendanceDetailPage));
        Routing.RegisterRoute(nameof(GradesDetailPage), typeof(GradesDetailPage));
        Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
    }
} 