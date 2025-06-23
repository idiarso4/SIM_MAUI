using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class StudentDetailPage : ContentPage
{
    public StudentDetailPage(StudentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is StudentDetailViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
} 