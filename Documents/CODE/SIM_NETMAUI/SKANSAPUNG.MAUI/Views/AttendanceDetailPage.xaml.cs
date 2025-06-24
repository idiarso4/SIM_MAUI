using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class AttendanceDetailPage : ContentPage
{
	public AttendanceDetailPage(AttendanceDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
