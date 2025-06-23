using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class SubjectsPage : ContentPage
{
    private readonly SubjectsViewModel _viewModel;

    public SubjectsPage(SubjectsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSubjectsCommand.ExecuteAsync(null);
    }
}
