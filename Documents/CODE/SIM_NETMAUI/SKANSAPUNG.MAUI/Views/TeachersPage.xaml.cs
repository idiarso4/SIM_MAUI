using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class TeachersPage : ContentPage
{
	public TeachersPage(TeachersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
} 