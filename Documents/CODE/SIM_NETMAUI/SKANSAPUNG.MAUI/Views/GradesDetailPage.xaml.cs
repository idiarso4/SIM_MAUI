using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class GradesDetailPage : ContentPage
{
	public GradesDetailPage(GradesDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
