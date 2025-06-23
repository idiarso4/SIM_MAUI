using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class TeachersViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IDatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Teacher> _teachers;

        [ObservableProperty]
        private Teacher _selectedTeacher;

        [ObservableProperty]
        private long _selectedDepartmentId;

        [ObservableProperty]
        private string _searchQuery;

        public TeachersViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
            : base(connectivityService)
        {
            _apiService = apiService;
            _databaseService = databaseService;
            Title = "Teachers";
            Teachers = new ObservableCollection<Teacher>();
        }

        [RelayCommand]
        private async Task LoadTeachersAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                if (!ConnectivityService.IsConnected())
                {
                    await ShowAlertAsync("No Internet", "Please check your internet connection.");
                    return;
                }

                var teachers = await _apiService.GetTeachersAsync();
                Teachers.Clear();
                foreach (var teacher in teachers)
                {
                    Teachers.Add(teacher);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Error", $"Failed to load teachers: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task FilterByDepartmentAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                if (!ConnectivityService.IsConnected())
                {
                    await ShowAlertAsync("No Internet", "Please check your internet connection.");
                    return;
                }

                var teachers = await _apiService.GetTeachersByDepartmentAsync(SelectedDepartmentId);
                Teachers.Clear();
                foreach (var teacher in teachers)
                {
                    Teachers.Add(teacher);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Error", $"Failed to filter teachers: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SearchTeachersAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(SearchQuery)) return;
            IsBusy = true;

            try
            {
                if (!ConnectivityService.IsConnected())
                {
                    await ShowAlertAsync("No Internet", "Please check your internet connection.");
                    return;
                }

                var teachers = await _apiService.SearchTeachersAsync(SearchQuery);
                Teachers.Clear();
                foreach (var teacher in teachers)
                {
                    Teachers.Add(teacher);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Error", $"Failed to search teachers: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task OnRefreshAsync()
        {
            await LoadTeachersAsync();
        }
    }
} 