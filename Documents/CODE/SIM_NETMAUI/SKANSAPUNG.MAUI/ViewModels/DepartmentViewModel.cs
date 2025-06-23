using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class DepartmentViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<LocalDepartment> _departments;

    [ObservableProperty]
    bool isRefreshing;

    public DepartmentViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        Title = "Departments";
        Departments = new ObservableCollection<LocalDepartment>();
    }

    [RelayCommand]
    public async Task LoadDepartmentsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Departments.Clear();

            if (ConnectivityService.IsConnected())
            {
                var apiList = await _apiService.GetDepartmentsAsync();
                foreach (var item in apiList)
                {
                    var local = new LocalDepartment
                    {
                        ServerId = item.Id.ToString(),
                        Name = item.Name,
                        Kode = item.Kode,
                        Status = item.Status,
                        IsSynced = true,
                        LastModified = DateTime.UtcNow
                    };
                    Departments.Add(local);
                    await _databaseService.SaveLocalDepartmentAsync(local);
                }
            }
            else
            {
                var localList = await _databaseService.GetLocalDepartmentsAsync();
                foreach (var local in localList)
                {
                    Departments.Add(local);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get departments: {ex.Message}");
            await ShowAlertAsync("Error!", ex.Message);
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}