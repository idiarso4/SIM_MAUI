using SKANSAPUNG.MAUI.ViewModels;
using CommunityToolkit.Maui.Behaviors;

namespace SKANSAPUNG.MAUI.Views;

public partial class AttendanceReportPage : ContentPage
{
	public AttendanceReportPage(AttendanceReportViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
