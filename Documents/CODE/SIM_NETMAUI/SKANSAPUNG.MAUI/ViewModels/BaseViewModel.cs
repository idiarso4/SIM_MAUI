using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using SKANSAPUNG.MAUI.Services;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private bool isRefreshing;

    public bool IsNotBusy => !IsBusy;

    protected readonly IConnectivityService ConnectivityService;

    public BaseViewModel(IConnectivityService connectivityService)
    {
        ConnectivityService = connectivityService;
    }

    // Konstruktor default untuk ViewModel yang tidak butuh service (jika ada)
    public BaseViewModel()
    {
        ConnectivityService = IPlatformApplication.Current.Services.GetService<IConnectivityService>();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await OnRefreshAsync();
        IsRefreshing = false;
    }

    protected virtual Task OnRefreshAsync()
    {
        return Task.CompletedTask;
    }

    protected async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }

    protected async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }
} 