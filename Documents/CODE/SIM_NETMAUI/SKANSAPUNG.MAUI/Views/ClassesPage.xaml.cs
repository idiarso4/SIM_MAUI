using SKANSAPUNG.MAUI.ViewModels;

namespace SKANSAPUNG.MAUI.Views;

public partial class ClassesPage : ContentPage
{
	public ClassesPage(ClassRoomViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
} 