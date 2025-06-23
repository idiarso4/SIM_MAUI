using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class NotificationsPage : ContentPage
{
	private readonly NotificationsViewModel _viewModel;
	public NotificationsPage(NotificationsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadNotificationsAsync();
	}
} 