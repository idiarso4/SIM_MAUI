using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class ReportsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private ReportSummaryDto _summary;

        public ReportsViewModel(IConnectivityService connectivityService, IApiService apiService)
            : base(connectivityService)
        {
            Title = "Laporan";
            _apiService = apiService;
            Summary = new ReportSummaryDto();
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