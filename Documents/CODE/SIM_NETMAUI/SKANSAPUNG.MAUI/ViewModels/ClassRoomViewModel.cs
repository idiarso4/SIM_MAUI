using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class ClassRoomViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<LocalClassRoom> _classRooms;

    [ObservableProperty]
    bool isRefreshing;

    public ClassRoomViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        Title = "Class Rooms";
        ClassRooms = new ObservableCollection<LocalClassRoom>();
    }

    [RelayCommand]
    public async Task GetClassRoomsAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            ClassRooms.Clear();
            if (IsInternetAvailable())
            {
                var apiList = await _apiService.GetClassRoomsAsync();
                foreach (var item in apiList)
                {
                    ClassRooms.Add(item);
                    // Simpan ke SQLite
                    var local = new LocalClassRoom
                    {
                        ServerId = item.Id.ToString(),
                        Name = item.Name,
                        Level = item.Level,
                        DepartmentId = item.DepartmentId,
                        SchoolYearId = item.SchoolYearId,
                        HomeroomTeacherId = item.HomeroomTeacherId,
                        IsActive = item.IsActive,
                        IsSynced = true,
                        LastModified = DateTime.UtcNow
                    };
                    await _databaseService.SaveLocalClassRoomAsync(local);
                }
            }
            else
            {
                // Offline: ambil dari SQLite
                var localList = await _databaseService.GetLocalClassRoomsAsync();
                foreach (var local in localList)
                {
                    ClassRooms.Add(new LocalClassRoom
                    {
                        ServerId = local.ServerId,
                        Name = local.Name,
                        Level = local.Level,
                        DepartmentId = local.DepartmentId,
                        SchoolYearId = local.SchoolYearId,
                        HomeroomTeacherId = local.HomeroomTeacherId,
                        IsActive = local.IsActive
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get class rooms: {ex.Message}");
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