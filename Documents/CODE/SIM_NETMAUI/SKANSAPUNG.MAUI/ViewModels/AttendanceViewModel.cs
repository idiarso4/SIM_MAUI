using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class AttendanceViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IGeolocationService _geolocationService;
    private readonly IAuthService _authService;
    private readonly IDatabaseService _databaseService;
    private readonly IConnectivityService _connectivityService;

    [ObservableProperty]
    private ObservableCollection<Attendance> attendances;

    [ObservableProperty]
    private DateTime selectedDate;

    [ObservableProperty]
    private bool isCheckInEnabled;

    [ObservableProperty]
    private bool isCheckOutEnabled;

    [ObservableProperty]
    private string currentLocation;

    [ObservableProperty]
    private string attendanceStatus;

    [ObservableProperty]
    private TimeSpan checkInTime;

    [ObservableProperty]
    private TimeSpan checkOutTime;

    [ObservableProperty]
    private bool isLocationAvailable;

    [ObservableProperty]
    private ObservableCollection<LocalAttendance> _attendanceHistory;

    [ObservableProperty]
    private bool _canCheckIn;

    [ObservableProperty]
    private bool _canCheckOut;

    public AttendanceViewModel(IApiService apiService, IGeolocationService geolocationService, IAuthService authService, IDatabaseService databaseService, IConnectivityService connectivityService) : base(connectivityService)
    {
        _apiService = apiService;
        _geolocationService = geolocationService;
        _authService = authService;
        _databaseService = databaseService;
        _connectivityService = connectivityService;
        Title = "Attendance";
        Attendances = new ObservableCollection<Attendance>();
        SelectedDate = DateTime.Today;
        AttendanceHistory = new ObservableCollection<LocalAttendance>();
    }

    public async Task InitializeAsync()
    {
        await LoadAttendanceDataAsync();
        await CheckLocationPermissionAsync();
    }

    private async Task LoadAttendanceDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null) return;

            var attendancesList = await _apiService.GetAttendancesAsync(currentUser.Id, SelectedDate);
            
            Attendances.Clear();
            foreach (var attendance in attendancesList)
            {
                Attendances.Add(attendance);
            }

            await UpdateAttendanceStatusAsync();
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Failed to load attendance data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task UpdateAttendanceStatusAsync()
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        if (currentUser == null) return;

        var todayAttendance = Attendances.FirstOrDefault(a => a.CreatedAt.Date == DateTime.Today);
        
        if (todayAttendance == null)
        {
            AttendanceStatus = "Not Checked In";
            IsCheckInEnabled = true;
            IsCheckOutEnabled = false;
            CheckInTime = TimeSpan.Zero;
            CheckOutTime = TimeSpan.Zero;
        }
        else if (todayAttendance.EndTime == TimeSpan.Zero)
        {
            AttendanceStatus = "Checked In";
            IsCheckInEnabled = false;
            IsCheckOutEnabled = true;
            CheckInTime = todayAttendance.StartTime;
            CheckOutTime = TimeSpan.Zero;
        }
        else
        {
            AttendanceStatus = "Checked Out";
            IsCheckInEnabled = false;
            IsCheckOutEnabled = false;
            CheckInTime = todayAttendance.StartTime;
            CheckOutTime = todayAttendance.EndTime;
        }
    }

    private async Task CheckLocationPermissionAsync()
    {
        try
        {
            var location = await _geolocationService.GetCurrentLocationAsync();
            IsLocationAvailable = location != null;
            
            if (location != null)
            {
                CurrentLocation = $"Lat: {location.Latitude:F6}, Lon: {location.Longitude:F6}";
            }
            else
            {
                CurrentLocation = "Location not available";
            }
        }
        catch (Exception ex)
        {
            IsLocationAvailable = false;
            CurrentLocation = "Location permission denied";
            System.Diagnostics.Debug.WriteLine($"Location error: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task CheckInAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            if (!IsLocationAvailable)
            {
                await ShowAlertAsync("Error", "Location is required for attendance. Please enable location services.");
                return;
            }

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null) return;

            var location = await _geolocationService.GetCurrentLocationAsync();
            if (location == null)
            {
                await ShowAlertAsync("Error", "Unable to get current location.");
                return;
            }

            var attendance = new Attendance
            {
                UserId = currentUser.Id,
                StartLatitude = location.Latitude,
                StartLongitude = location.Longitude,
                StartTime = DateTime.Now.TimeOfDay,
                CreatedAt = DateTime.Now
            };

            // Simpan ke SQLite terlebih dahulu (offline-first)
            var localAttendance = new LocalAttendance
            {
                UserId = currentUser.Id,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                CheckInTime = DateTime.Now,
                IsSynced = false,
                LastModified = DateTime.UtcNow
            };
            await _databaseService.SaveLocalAttendanceAsync(localAttendance);

            // Coba sync ke server jika online
            if (IsInternetAvailable())
            {
                try
                {
                    var result = await _apiService.CreateAttendanceAsync(attendance);
                    if (result.IsSuccess)
                    {
                        // Update status sync di SQLite
                        localAttendance.IsSynced = true;
                        localAttendance.ServerId = result.Data?.ToString();
                        await _databaseService.SaveLocalAttendanceAsync(localAttendance);
                        await ShowAlertAsync("Success", "Check-in successful!");
                    }
                    else
                    {
                        await ShowAlertAsync("Warning", "Check-in saved locally. Will sync when online.");
                    }
                }
                catch
                {
                    await ShowAlertAsync("Warning", "Check-in saved locally. Will sync when online.");
                }
            }
            else
            {
                await ShowAlertAsync("Success", "Check-in saved locally. Will sync when online.");
            }

            await LoadAttendanceDataAsync();
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Check-in failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CheckOutAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            if (!IsLocationAvailable)
            {
                await ShowAlertAsync("Error", "Location is required for attendance. Please enable location services.");
                return;
            }

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null) return;

            var location = await _geolocationService.GetCurrentLocationAsync();
            if (location == null)
            {
                await ShowAlertAsync("Error", "Unable to get current location.");
                return;
            }

            // Cari attendance hari ini (dari SQLite atau API)
            var todayAttendance = await GetTodayAttendanceAsync();
            if (todayAttendance == null)
            {
                await ShowAlertAsync("Error", "No check-in record found for today.");
                return;
            }

            // Update checkout di SQLite
            var localAttendance = await GetLocalAttendanceForTodayAsync();
            if (localAttendance != null)
            {
                localAttendance.CheckOutTime = DateTime.Now;
                localAttendance.IsSynced = false;
                localAttendance.LastModified = DateTime.UtcNow;
                await _databaseService.SaveLocalAttendanceAsync(localAttendance);
            }

            // Coba sync ke server jika online
            if (IsInternetAvailable())
            {
                try
                {
                    todayAttendance.EndLatitude = location.Latitude;
                    todayAttendance.EndLongitude = location.Longitude;
                    todayAttendance.EndTime = DateTime.Now.TimeOfDay;

                    var result = await _apiService.UpdateAttendanceAsync(todayAttendance);
                    if (result.IsSuccess)
                    {
                        if (localAttendance != null)
                        {
                            localAttendance.IsSynced = true;
                            await _databaseService.SaveLocalAttendanceAsync(localAttendance);
                        }
                        await ShowAlertAsync("Success", "Check-out successful!");
                    }
                    else
                    {
                        await ShowAlertAsync("Warning", "Check-out saved locally. Will sync when online.");
                    }
                }
                catch
                {
                    await ShowAlertAsync("Warning", "Check-out saved locally. Will sync when online.");
                }
            }
            else
            {
                await ShowAlertAsync("Success", "Check-out saved locally. Will sync when online.");
            }

            await LoadAttendanceDataAsync();
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Check-out failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SyncAttendanceAsync()
    {
        if (IsBusy) return;

        // Periksa konektivitas sebelum mencoba sinkronisasi
        if (!ConnectivityService.IsConnected)
        {
            await Shell.Current.DisplayAlert("Offline", "Tidak ada koneksi internet. Sinkronisasi akan dicoba lagi nanti.", "OK");
            return;
        }

        IsBusy = true;
        Title = "Syncing...";

        try
        {
            // 1. Dapatkan data yang belum disinkronkan dari database lokal
            var unsyncedAttendances = await _databaseService.GetUnsyncedAttendancesAsync();

            if (unsyncedAttendances == null || !unsyncedAttendances.Any())
            {
                await Shell.Current.DisplayAlert("Sync", "Tidak ada data absensi untuk disinkronkan.", "OK");
                return;
            }

            // 2. Kirim data ke API
            var syncedIds = await _apiService.SyncAttendanceAsync(unsyncedAttendances);

            // 3. Jika ada data yang berhasil disinkronkan, perbarui database lokal
            if (syncedIds != null && syncedIds.Any())
            {
                var localIdsToUpdate = syncedIds.Select(int.Parse).ToList();
                await _databaseService.MarkAttendancesAsSyncedAsync(localIdsToUpdate);

                await Shell.Current.DisplayAlert("Success", $"{syncedIds.Count} data absensi berhasil disinkronkan.", "OK");

                // Muat ulang data di halaman untuk mencerminkan perubahan
                await LoadAttendanceHistoryAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Sync Failed", "Gagal menyinkronkan data. Silakan coba lagi.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Terjadi kesalahan saat sinkronisasi: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            Title = "Attendance";
        }
    }

    private async Task<Attendance> GetTodayAttendanceAsync()
    {
        // Coba ambil dari API dulu
        if (IsInternetAvailable())
        {
            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                var attendances = await _apiService.GetAttendancesAsync(currentUser.Id, DateTime.Today);
                return attendances.FirstOrDefault(a => a.CreatedAt.Date == DateTime.Today);
            }
            catch
            {
                // Fallback ke SQLite
            }
        }

        // Ambil dari SQLite
        var localAttendance = await GetLocalAttendanceForTodayAsync();
        if (localAttendance != null)
        {
            return new Attendance
            {
                Id = long.TryParse(localAttendance.ServerId, out var sid) ? sid : 0,
                UserId = localAttendance.UserId,
                StartLatitude = localAttendance.Latitude,
                StartLongitude = localAttendance.Longitude,
                StartTime = localAttendance.CheckInTime.TimeOfDay,
                EndTime = localAttendance.CheckOutTime?.TimeOfDay ?? TimeSpan.Zero,
                CreatedAt = localAttendance.CheckInTime
            };
        }

        return null;
    }

    private async Task<LocalAttendance> GetLocalAttendanceForTodayAsync()
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        if (currentUser == null) return null;

        var allAttendances = await _databaseService.GetLocalAttendancesAsync();
        return allAttendances.FirstOrDefault(a => 
            a.UserId == currentUser.Id && 
            a.CheckInTime.Date == DateTime.Today);
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

    [RelayCommand]
    private async Task RefreshLocationAsync()
    {
        await CheckLocationPermissionAsync();
    }

    [RelayCommand]
    private async Task ViewAttendanceHistoryAsync()
    {
        await Shell.Current.GoToAsync("AttendanceHistory");
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadAttendanceDataAsync();
    }

    protected override async Task OnRefreshAsync()
    {
        await LoadAttendanceDataAsync();
        await CheckLocationPermissionAsync();
    }
} 