using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Check if user is already logged in
        var authService = Handler?.MauiContext?.Services.GetService<IAuthService>();
        if (authService != null)
        {
            var currentUser = await authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                await Shell.Current.GoToAsync("///Dashboard");
            }
        }
    }
} 