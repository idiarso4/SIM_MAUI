using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class ExtracurricularPage : ContentPage
{
	private readonly ExtracurricularViewModel _viewModel;
	public ExtracurricularPage(ExtracurricularViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.GetExtracurricularsCommand.ExecuteAsync(null);
    }
} 