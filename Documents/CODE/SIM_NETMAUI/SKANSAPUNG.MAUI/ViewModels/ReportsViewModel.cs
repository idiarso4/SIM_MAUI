using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class ReportsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private ReportSummaryDto _summary;

        public ObservableCollection<ReportType> AvailableReports { get; } = new();

        public ReportsViewModel(IConnectivityService connectivityService, IApiService apiService)
            : base(connectivityService)
        {
            Title = "Laporan";
            _apiService = apiService;
            Summary = new ReportSummaryDto();
            LoadAvailableReports();
        }

        [RelayCommand]
        private void LoadAvailableReports()
        {
            AvailableReports.Add(new ReportType { Title = "Laporan Kehadiran", Description = "Lihat rekapitulasi kehadiran siswa.", Icon = "attendance.png", TargetPageRoute = "AttendanceReportPage" });
            AvailableReports.Add(new ReportType { Title = "Laporan Nilai", Description = "Lihat rekapitulasi nilai siswa per kelas.", Icon = "grades.svg", TargetPageRoute = "GradesReportPage" });
            // Tambahkan jenis laporan lain di sini jika perlu
        }

        [RelayCommand]
        private async Task SelectReportAsync(ReportType reportType)
        {
            if (reportType == null)
                return;

            // Navigasi ke halaman detail laporan yang sesuai
            await Shell.Current.GoToAsync(reportType.TargetPageRoute, true, new Dictionary<string, object>
            {
                { "ReportTitle", reportType.Title }
            });
        }

        [RelayCommand]
        private async Task LoadSummaryAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                if (ConnectivityService.IsConnected)
                {
                    var summary = await _apiService.GetReportSummaryAsync();
                    if (summary != null)
                    {
                        Summary = summary;
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Offline", "Data laporan hanya tersedia saat online.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading report summary: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Gagal memuat ringkasan laporan.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 