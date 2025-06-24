using CommunityToolkit.Mvvm.ComponentModel;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    [QueryProperty(nameof(StudentId), "StudentId")]
    [QueryProperty(nameof(StudentName), "StudentName")]
    public partial class AttendanceDetailViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private string _studentId;

        [ObservableProperty]
        private string _studentName;

        [ObservableProperty]
        private ObservableCollection<AttendanceRecord> _attendanceRecords;

        public AttendanceDetailViewModel(IApiService apiService, IConnectivityService connectivityService)
            : base(connectivityService)
        {
            _apiService = apiService;
            Title = "Detail Absensi";
            AttendanceRecords = new ObservableCollection<AttendanceRecord>();
        }

        partial void OnStudentIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Task.Run(LoadAttendanceDataAsync);
            }
        }

        private async Task LoadAttendanceDataAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                if (!ConnectivityService.IsConnected())
                {
                    await ShowAlertAsync("No Internet", "Please check your internet connection.");
                    return;
                }

                var records = await _apiService.GetAttendanceDetailAsync(StudentId);
                if (records != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AttendanceRecords.Clear();
                        foreach (var record in records)
                        {
                            AttendanceRecords.Add(record);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading attendance data: {ex.Message}");
                await ShowAlertAsync("Error", "Failed to load attendance details.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
