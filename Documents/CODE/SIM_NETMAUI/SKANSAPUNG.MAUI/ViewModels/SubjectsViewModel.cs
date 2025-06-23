using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class SubjectsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<Subject> _subjects;

        [ObservableProperty]
        private Subject _selectedSubject;

        [ObservableProperty]
        private long _selectedDepartmentId;

        public SubjectsViewModel(IApiService apiService, IConnectivityService connectivityService)
            : base(connectivityService)
        {
            _apiService = apiService;
            Subjects = new ObservableCollection<Subject>();
            Title = "Subjects";
        }

        [RelayCommand]
        private async Task LoadSubjectsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (!ConnectivityService.IsConnected())
                {
                    await ShowAlertAsync("No Internet", "Please check your internet connection.");
                    return;
                }

                var subjects = await _apiService.GetSubjectsAsync();

                Subjects.Clear();
                foreach (var subject in subjects)
                {
                    Subjects.Add(subject);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Error", $"Failed to load subjects: {ex.Message}");
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

            try
            {
                IsBusy = true;

                if (!ConnectivityService.IsConnected())
                {
                    await ShowAlertAsync("No Internet", "Please check your internet connection.");
                    return;
                }

                var subjects = await _apiService.GetSubjectsByDepartmentAsync(SelectedDepartmentId);

                Subjects.Clear();
                foreach (var subject in subjects)
                {
                    Subjects.Add(subject);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Error", $"Failed to filter subjects: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ViewSubjectDetailsAsync(Subject subject)
        {
            if (subject == null) return;

            SelectedSubject = subject;

            // Navigate to subject details page
            var parameters = new Dictionary<string, object>
            {
                { "Subject", subject }
            };

            await Shell.Current.GoToAsync("SubjectDetailPage", parameters);
        }

        protected override async Task OnRefreshAsync()
        {
            await LoadSubjectsAsync();
        }
    }
}
