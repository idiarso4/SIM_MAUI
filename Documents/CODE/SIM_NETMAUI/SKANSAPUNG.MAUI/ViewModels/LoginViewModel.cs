using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;
    private readonly IDatabaseService _databaseService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private bool _isLoggingIn;

    public LoginViewModel(IApiService apiService, IAuthService authService, IDatabaseService databaseService, 
        INotificationService notificationService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _authService = authService;
        _databaseService = databaseService;
        _notificationService = notificationService;
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsLoggingIn || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await ShowAlertAsync("Error", "Please enter both username and password.");
            return;
        }

        try
        {
            IsLoggingIn = true;

            if (!ConnectivityService.IsConnected())
            {
                await ShowAlertAsync("No Internet", "Please check your internet connection.");
                return;
            }

            var user = await _apiService.LoginAsync(Username, Password);
            if (user != null)
            {
                // Save user locally
                await _databaseService.SaveUserAsync(new LocalUser
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    Role = user.Role,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });

                // Register for push notifications
                try
                {
                    var token = await _notificationService.RegisterForPushNotificationsAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        await _apiService.RegisterTokenAsync(token, DeviceInfo.Current.Model);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error registering for push notifications: {ex.Message}");
                }

                // Navigate to dashboard
                await Shell.Current.GoToAsync("///dashboard");
            }
            else
            {
                await ShowAlertAsync("Login Failed", "Invalid username or password.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login error: {ex.Message}");
            await ShowAlertAsync("Login Error", "An error occurred during login. Please try again.");
        }
        finally
        {
            IsLoggingIn = false;
        }
    }

    [RelayCommand]
    private async Task CheckLoginStatusAsync()
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                // Check if token is still valid
                if (ConnectivityService.IsConnected())
                {
                    var isValid = await _apiService.ValidateTokenAsync();
                    if (isValid)
                    {
                        await Shell.Current.GoToAsync("///dashboard");
                        return;
                    }
                    else
                    {
                        // Token invalid, logout
                        await _authService.LogoutAsync();
                    }
                }
                else
                {
                    // Offline but we have a user, go to dashboard
                    await Shell.Current.GoToAsync("///dashboard");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking login status: {ex.Message}");
        }
    }
} 