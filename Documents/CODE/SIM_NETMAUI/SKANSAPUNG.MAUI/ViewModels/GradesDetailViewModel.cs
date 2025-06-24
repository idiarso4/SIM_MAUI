using CommunityToolkit.Mvvm.ComponentModel;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    [QueryProperty(nameof(StudentId), "StudentId")]
    [QueryProperty(nameof(StudentName), "StudentName")]
    [QueryProperty(nameof(SubjectName), "SubjectName")]
    public partial class GradesDetailViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        private string _studentId;

        [ObservableProperty]
        private string _studentName;

        [ObservableProperty]
        private string _subjectName;

        public ObservableCollection<GradeRecord> GradeRecords { get; } = new();

        public GradesDetailViewModel(IApiService apiService, IConnectivityService connectivityService)
            : base(connectivityService)
        {
            _apiService = apiService;
            Title = "Detail Nilai";
        }

        // Using a combined method to trigger data loading once all properties are set.
        public override async Task OnNavigatedTo(IDictionary<string, object> parameters)
        {
            await base.OnNavigatedTo(parameters);
            if (!string.IsNullOrEmpty(StudentId) && !string.IsNullOrEmpty(SubjectName))
            {
                await LoadGradeDataAsync();
            }
        }

        private async Task LoadGradeDataAsync()
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

                var records = await _apiService.GetGradesDetailAsync(StudentId, SubjectName);
                if (records != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        GradeRecords.Clear();
                        foreach (var record in records)
                        {
                            GradeRecords.Add(record);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading grade data: {ex.Message}");
                await ShowAlertAsync("Error", "Failed to load grade details.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
