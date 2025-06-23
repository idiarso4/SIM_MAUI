using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class StudentDetailViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private Student _student;

    [ObservableProperty]
    private int studentId;

    public StudentDetailViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        Title = "Student Detail";
    }

    public async Task InitializeAsync()
    {
        await LoadStudentDetailAsync();
    }

    private async Task LoadStudentDetailAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Student = await _apiService.GetStudentByIdAsync(StudentId);
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Failed to load student details: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditStudentAsync()
    {
        if (Student == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "StudentId", Student.Id }
        };

        await Shell.Current.GoToAsync("EditStudent", parameters);
    }

    protected override async Task OnRefreshAsync()
    {
        await LoadStudentDetailAsync();
    }
} 