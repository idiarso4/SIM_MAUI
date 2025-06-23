using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SKANSAPUNG.MAUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GradesPage : ContentPage
    {
        private GradesPageViewModel _viewModel;

        public GradesPage(GradesPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }
    }
} 