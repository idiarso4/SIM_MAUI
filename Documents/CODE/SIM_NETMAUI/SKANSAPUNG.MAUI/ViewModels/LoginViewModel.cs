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
    private readonly IBiometricService _biometricService;

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;



    [ObservableProperty]
    private bool _isBiometricAvailable;

    public LoginViewModel(IApiService apiService, IAuthService authService, IDatabaseService databaseService, 
        INotificationService notificationService, IConnectivityService connectivityService, IBiometricService biometricService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _authService = authService;
        _databaseService = databaseService;
        _notificationService = notificationService;
        _biometricService = biometricService;
        Title = "Login";

        // Check for biometric availability on startup
        Task.Run(async () => IsBiometricAvailable = await _biometricService.IsAvailableAsync() && !string.IsNullOrEmpty(await _authService.GetTokenAsync()));
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await ShowAlertAsync("Error", "Please enter both username and password.");
            return;
        }

        try
        {
            IsBusy = true;

            if (!ConnectivityService.IsConnected())
            {
                await ShowAlertAsync("No Internet", "Please check your internet connection.");
                return;
            }

            var user = await _apiService.LoginAsync(Username, Password);
            if (user != null && !string.IsNullOrEmpty(user.RememberToken))
            {
                // Save authentication state
                await _authService.SaveTokenAsync(user.RememberToken);
                await _authService.SaveUserAsync(user);

                // The original code saved a LocalUser, let's keep that logic.
                // NOTE: The User model does not contain Username or PhoneNumber. Using Email for Username.
                await _databaseService.SaveUserAsync(new LocalUser
                {
                    Id = user.Id,
                    Username = user.Email, 
                    Name = user.Name,
                    Role = user.Role,
                    Email = user.Email,
                    PhoneNumber = ""
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

                // Notify AppShell to update tabs
                MessagingCenter.Send<object>(this, "UpdateTabs");

                // Navigate to the main tabbed page
                await Shell.Current.GoToAsync("//MainTabs");
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
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task BiometricLoginAsync()
    {
        if (IsBusy || !IsBiometricAvailable)
            return;

        try
        {
            IsBusy = true;

            var authenticated = await _biometricService.AuthenticateAsync("Login to SKANSAPUNG");
            if (authenticated)
            {
                var tokenValid = await _apiService.ValidateTokenAsync();
                if (tokenValid)
                {
                    MessagingCenter.Send<object>(this, "UpdateTabs");
                    await Shell.Current.GoToAsync("//MainTabs");
                }
                else
                {
                    await ShowAlertAsync("Session Expired", "Your session has expired. Please log in again.");
                    await _authService.LogoutAsync(); // Clear expired token
                }
            }
            else
            {
                await ShowAlertAsync("Authentication Failed", "Could not verify your identity.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Biometric login error: {ex.Message}");
            await ShowAlertAsync("Error", "An unexpected error occurred during biometric login.");
        }
        finally
        {
            IsBusy = false;
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