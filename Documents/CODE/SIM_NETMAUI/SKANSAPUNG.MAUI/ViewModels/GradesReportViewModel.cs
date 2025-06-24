using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels;

[QueryProperty(nameof(ReportTitle), "ReportTitle")]
public partial class GradesReportViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private string reportTitle;

    public ObservableCollection<StudentGradeRecordDto> GradeRecords { get; } = new();

    public GradesReportViewModel(IConnectivityService connectivityService, IApiService apiService) : base(connectivityService)
    {
        _apiService = apiService;
        // Judul akan diatur melalui QueryProperty saat navigasi
    }

    [RelayCommand]
    private async Task LoadGradesReportAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            // Sama seperti sebelumnya, kita hardcode ID kelas untuk saat ini.
            long classRoomId = 1; // Ganti dengan ID kelas yang relevan
            var records = await _apiService.GetGradesReportAsync(classRoomId);

            GradeRecords.Clear();
            foreach (var record in records)
            {
                GradeRecords.Add(record);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading grades report: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Gagal memuat laporan nilai.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToDetailsAsync(StudentGradeRecordDto record)
    {
        if (record == null || string.IsNullOrEmpty(record.StudentId))
            return;

        try
        {
            await Shell.Current.GoToAsync(nameof(GradesDetailPage), true, new Dictionary<string, object>
            {
                { "StudentId", record.StudentId },
                { "StudentName", record.StudentName },
                { "SubjectName", record.SubjectName } // Assuming SubjectName exists in the DTO
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to navigate to grade detail page: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Tidak dapat membuka halaman detail nilai.", "OK");
        }
    }
}
