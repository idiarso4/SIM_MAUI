using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private User currentUser;

    [ObservableProperty]
    private LocalUser _user;

    public bool SimulateOffline
    {
        get => false; // Selalu false agar tidak error saat get
        set
        {
            if (ConnectivityService != null)
            {
                ConnectivityService.SimulateOffline(value);
                Shell.Current.DisplayAlert("Debug", $"Simulate Offline: {value}", "OK");
            }
        }
    }

    public ProfileViewModel(IAuthService authService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _authService = authService;
        _databaseService = databaseService;
        Title = "Profile";
    }

    public async Task InitializeAsync()
    {
        await LoadUserProfileAsync();
    }

    private async Task LoadUserProfileAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            if (IsInternetAvailable())
            {
                CurrentUser = await _authService.GetCurrentUserAsync();
                if (CurrentUser != null)
                {
                    var local = new LocalUser
                    {
                        ServerId = CurrentUser.Id.ToString(),
                        Name = CurrentUser.Name,
                        Email = CurrentUser.Email,
                        Role = CurrentUser.Role,
                        UserType = CurrentUser.UserType,
                        Status = CurrentUser.Status,
                        Image = CurrentUser.Image,
                        IsSynced = true,
                        LastModified = DateTime.UtcNow
                    };
                    await _databaseService.SaveLocalUserAsync(local);
                }
            }
            else
            {
                // Offline: ambil dari SQLite
                var localList = await _databaseService.GetLocalUsersAsync();
                var local = localList.FirstOrDefault();
                if (local != null)
                {
                    CurrentUser = new User
                    {
                        Id = long.TryParse(local.ServerId, out var sid) ? sid : 0,
                        Name = local.Name,
                        Email = local.Email,
                        Role = local.Role,
                        UserType = local.UserType,
                        Status = local.Status,
                        Image = local.Image
                    };
                }
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Failed to load profile: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
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

    [RelayCommand]
    private async Task EditProfileAsync()
    {
        await Shell.Current.GoToAsync("EditProfile");
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        await Shell.Current.GoToAsync("ChangePassword");
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        var confirmed = await App.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (confirmed)
        {
            await _authService.LogoutAsync();

            // Notify AppShell to clear tabs
            MessagingCenter.Send<object>(this, "UpdateTabs");

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }

    protected override async Task OnRefreshAsync()
    {
        await LoadUserProfileAsync();
    }
} 