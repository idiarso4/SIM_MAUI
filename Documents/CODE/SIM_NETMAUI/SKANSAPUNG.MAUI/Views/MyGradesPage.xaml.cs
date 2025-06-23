using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class MyGradesPage : ContentPage
{
	public MyGradesPage(MyGradesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
} 