using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class AttendancePage : ContentPage
{
    public AttendancePage(AttendanceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is AttendanceViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
} 