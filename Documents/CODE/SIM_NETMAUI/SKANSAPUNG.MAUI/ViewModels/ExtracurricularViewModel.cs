using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class ExtracurricularViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IDatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<LocalExtracurricular> _extracurriculars;

        [ObservableProperty]
        bool isRefreshing;

        public ExtracurricularViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
            : base(connectivityService)
        {
            _apiService = apiService;
            _databaseService = databaseService;
            Title = "Extracurriculars";
            Extracurriculars = new ObservableCollection<LocalExtracurricular>();
        }

        [RelayCommand]
        async Task GetExtracurricularsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Extracurriculars.Clear();
                if (IsInternetAvailable())
                {
                    var extracurricularList = await _apiService.GetExtracurricularsAsync();
                    foreach (var item in extracurricularList)
                    {
                        Extracurriculars.Add(item);
                        // Simpan ke SQLite
                        var local = new LocalExtracurricular
                        {
                            ServerId = item.Id.ToString(),
                            Nama = item.Nama,
                            Deskripsi = item.Deskripsi,
                            Hari = item.Hari,
                            JamMulai = item.JamMulai.ToString(),
                            JamSelesai = item.JamSelesai.ToString(),
                            Tempat = item.Tempat,
                            Status = item.Status,
                            IsSynced = true,
                            LastModified = DateTime.UtcNow
                        };
                        await _databaseService.SaveLocalExtracurricularAsync(local);
                    }
                }
                else
                {
                    // Offline: ambil dari SQLite
                    var localList = await _databaseService.GetLocalExtracurricularsAsync();
                    foreach (var local in localList)
                    {
                        Extracurriculars.Add(new LocalExtracurricular
                        {
                            ServerId = local.ServerId,
                            Nama = local.Nama,
                            Deskripsi = local.Deskripsi,
                            Hari = local.Hari,
                            JamMulai = TimeSpan.TryParse(local.JamMulai, out var jm) ? jm : TimeSpan.Zero,
                            JamSelesai = TimeSpan.TryParse(local.JamSelesai, out var js) ? js : TimeSpan.Zero,
                            Tempat = local.Tempat,
                            Status = local.Status
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get extracurriculars: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private bool IsInternetAvailable()
        {
            try
            {
                return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            }
            catch
            {
                return false;
            }
        }
    }
} 