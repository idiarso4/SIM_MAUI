using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class DepartmentPage : ContentPage
{
    private readonly DepartmentViewModel _viewModel;

    public DepartmentPage(DepartmentViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDepartmentsCommand.ExecuteAsync(null);
    }
}
