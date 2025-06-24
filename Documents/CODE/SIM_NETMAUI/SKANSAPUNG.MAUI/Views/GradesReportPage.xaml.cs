using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class GradesReportPage : ContentPage
{
	public GradesReportPage(GradesReportViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
