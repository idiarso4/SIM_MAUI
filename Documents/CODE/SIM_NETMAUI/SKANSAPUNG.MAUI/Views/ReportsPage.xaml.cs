using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class ReportsPage : ContentPage
{
	public ReportsPage(ReportsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
} 