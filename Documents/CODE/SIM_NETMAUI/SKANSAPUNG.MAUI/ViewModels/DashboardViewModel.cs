using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private LocalUser _currentUser;

    [ObservableProperty]
    private int _unreadNotificationsCount;

    [ObservableProperty]
    private ObservableCollection<LocalNotification> _recentNotifications;

    [ObservableProperty]
    private ObservableCollection<ScheduleItem> _todaySchedule;

    [ObservableProperty]
    private string _welcomeMessage;

    [ObservableProperty]
    private bool _isTeacher;

    [ObservableProperty]
    private bool _isStudent;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private int _totalStudents;

    [ObservableProperty]
    private int _totalTeachers;

    [ObservableProperty]
    private int _totalClasses;

    public DashboardViewModel(IApiService apiService, IAuthService authService, IDatabaseService databaseService, 
        IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _authService = authService;
        _databaseService = databaseService;
        Title = "Dashboard";
        RecentNotifications = new ObservableCollection<LocalNotification>();
        TodaySchedule = new ObservableCollection<ScheduleItem>();
    }

    [RelayCommand]
    private async Task LoadDashboardDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Get current user
            CurrentUser = await _authService.GetCurrentUserAsync();
            if (CurrentUser == null)
            {
                // Not logged in, redirect to login
                await Shell.Current.GoToAsync("///login");
                return;
            }

            // Set role-based flags
            IsTeacher = CurrentUser.Role?.ToLower() == "teacher";
            IsStudent = CurrentUser.Role?.ToLower() == "student";
            IsAdmin = CurrentUser.Role?.ToLower() == "admin";

            // Set welcome message based on time of day
            var hour = DateTime.Now.Hour;
            if (hour < 12)
                WelcomeMessage = $"Good Morning, {CurrentUser.Name}";
            else if (hour < 18)
                WelcomeMessage = $"Good Afternoon, {CurrentUser.Name}";
            else
                WelcomeMessage = $"Good Evening, {CurrentUser.Name}";

            // Load notifications
            await LoadNotificationsAsync();

            // Load today's schedule
            await LoadTodayScheduleAsync();

            // Load statistics if admin
            if (IsAdmin && ConnectivityService.IsConnected())
            {
                await LoadStatisticsAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
            await ShowAlertAsync("Error", "Failed to load dashboard data.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadNotificationsAsync()
    {
        try
        {
            var localNotifications = await _databaseService.GetNotificationsAsync(CurrentUser.Id);
            
            // Count unread notifications
            UnreadNotificationsCount = localNotifications.Count(n => !n.IsRead);
            
            // Get recent notifications (last 5)
            RecentNotifications.Clear();
            foreach (var notification in localNotifications.OrderByDescending(n => n.CreatedAt).Take(5))
            {
                RecentNotifications.Add(notification);
            }
            
            // Sync with server if online
            if (ConnectivityService.IsConnected())
            {
                var apiNotifications = await _apiService.GetNotificationsByUserAsync(CurrentUser.Id);
                await _databaseService.SaveNotificationsAsync(apiNotifications.Select(n => new LocalNotification
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead,
                    UserId = n.UserId
                }).ToList());
                
                // Refresh from database after sync
                localNotifications = await _databaseService.GetNotificationsAsync(CurrentUser.Id);
                UnreadNotificationsCount = localNotifications.Count(n => !n.IsRead);
                
                RecentNotifications.Clear();
                foreach (var notification in localNotifications.OrderByDescending(n => n.CreatedAt).Take(5))
                {
                    RecentNotifications.Add(notification);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading notifications: {ex.Message}");
        }
    }

    private async Task LoadTodayScheduleAsync()
    {
        try
        {
            var today = DateTime.Today.DayOfWeek.ToString();
            
            if (ConnectivityService.IsConnected())
            {
                List<Schedule> schedules = new List<Schedule>();
                
                if (IsTeacher)
                {
                    // Get teacher's schedule
                    schedules = await _apiService.GetSchedulesByTeacherAsync(CurrentUser.Id);
                }
                else if (IsStudent)
                {
                    // Get student's class schedule
                    var studentDetails = await _apiService.GetStudentDetailsAsync(CurrentUser.Id);
                    if (studentDetails?.ClassRoomId != null)
                    {
                        schedules = await _apiService.GetSchedulesByClassAsync(studentDetails.ClassRoomId.Value);
                    }
                }
                else if (IsAdmin)
                {
                    // Get all schedules
                    schedules = await _apiService.GetSchedulesAsync();
                }
                
                // Filter for today's schedule
                var todayItems = schedules
                    .Where(s => s.Day.ToLower() == today.ToLower())
                    .OrderBy(s => s.StartTime)
                    .Select(s => new ScheduleItem
                    {
                        Id = s.Id,
                        Subject = s.Subject?.Name ?? "Unknown Subject",
                        Teacher = s.Teacher?.Name ?? "Unknown Teacher",
                        ClassRoom = s.ClassRoom?.Name ?? "Unknown Class",
                        Day = s.Day,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime
                    })
                    .ToList();
                
                TodaySchedule.Clear();
                foreach (var item in todayItems)
                {
                    TodaySchedule.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading schedule: {ex.Message}");
        }
    }

    private async Task LoadStatisticsAsync()
    {
        try
        {
            // Get counts from API
            var students = await _apiService.GetStudentsAsync();
            TotalStudents = students.Count;
            
            var teachers = await _apiService.GetTeachersAsync();
            TotalTeachers = teachers.Count;
            
            var classes = await _apiService.GetClassRoomsAsync();
            TotalClasses = classes.Count;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading statistics: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        try
        {
            var result = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (result)
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("///login");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during logout: {ex.Message}");
            await ShowAlertAsync("Error", "Failed to logout.");
        }
    }

    protected override async Task OnRefreshAsync()
    {
        await LoadDashboardDataAsync();
    }

    public void OnAppearing()
    {
        // Implementation of OnAppearing method
    }

    [RelayCommand]
    private async Task GoToStudents() => await Shell.Current.GoToAsync(nameof(Views.StudentsPage));

    [RelayCommand]
    private async Task GoToClasses() => await Shell.Current.GoToAsync(nameof(Views.ClassesPage));

    [RelayCommand]
    private async Task GoToTeachers() => await Shell.Current.GoToAsync(nameof(Views.TeachersPage));

    [RelayCommand]
    private async Task GoToExtracurricular() => await Shell.Current.GoToAsync(nameof(Views.ExtracurricularPage));

    [RelayCommand]
    private async Task GoToSchedule() => await Shell.Current.GoToAsync(nameof(Views.SchedulePage));

    [RelayCommand]
    private async Task GoToGrades() => await Shell.Current.GoToAsync(nameof(Views.GradesPage));

    [RelayCommand]
    private async Task GoToReports() => await Shell.Current.GoToAsync(nameof(Views.ReportsPage));

    [RelayCommand]
    private async Task GoToNotifications() => await Shell.Current.GoToAsync(nameof(Views.NotificationsPage));
}

public class DashboardItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string Route { get; set; }
} 