using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels;

[QueryProperty(nameof(ReportTitle), "ReportTitle")]
public partial class AttendanceReportViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private string reportTitle;

    public ObservableCollection<StudentAttendanceRecordDto> AttendanceRecords { get; } = new();

    public AttendanceReportViewModel(IConnectivityService connectivityService, IApiService apiService) : base(connectivityService)
    {
        _apiService = apiService;
        // Judul akan diatur melalui QueryProperty saat navigasi
    }

    [RelayCommand]
    private async Task LoadAttendanceReportAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            // Untuk sekarang, kita hardcode ID kelas. Nantinya ini bisa dibuat dinamis.
            long classRoomId = 1; // Ganti dengan ID kelas yang relevan
            var records = await _apiService.GetAttendanceReportAsync(classRoomId);

            AttendanceRecords.Clear();
            foreach (var record in records)
            {
                AttendanceRecords.Add(record);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading attendance report: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Gagal memuat laporan kehadiran.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(StudentAttendanceRecordDto record)
    {
        if (record == null || string.IsNullOrEmpty(record.StudentId))
            return;

        try
        {
            await Shell.Current.GoToAsync(nameof(AttendanceDetailPage), true, new Dictionary<string, object>
            {
                { "StudentId", record.StudentId },
                { "StudentName", record.StudentName }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to navigate to detail page: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Tidak dapat membuka halaman detail.", "OK");
        }
    }
}
