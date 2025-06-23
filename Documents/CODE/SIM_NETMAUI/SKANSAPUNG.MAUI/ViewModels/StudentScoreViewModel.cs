using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class StudentScoreViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<LocalStudentScore> _studentScores;

    [ObservableProperty]
    bool isRefreshing;

    public StudentScoreViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        Title = "Student Scores";
        StudentScores = new ObservableCollection<LocalStudentScore>();
    }

    [RelayCommand]
    public async Task GetStudentScoresAsync(long studentId)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            StudentScores.Clear();
            if (IsInternetAvailable())
            {
                var apiList = await _apiService.GetStudentScoresAsync(studentId);
                foreach (var item in apiList)
                {
                    StudentScores.Add(item);
                    // Simpan ke SQLite
                    var local = new LocalStudentScore
                    {
                        ServerId = item.Id.ToString(),
                        AssessmentId = item.AssessmentId,
                        StudentId = item.StudentId,
                        Score = item.Score,
                        Status = item.Status.ToString(),
                        IsSynced = true,
                        LastModified = DateTime.UtcNow
                    };
                    await _databaseService.SaveLocalStudentScoreAsync(local);
                }
            }
            else
            {
                // Offline: ambil dari SQLite
                var localList = await _databaseService.GetLocalStudentScoresAsync();
                foreach (var local in localList.Where(x => x.StudentId == studentId))
                {
                    StudentScores.Add(new LocalStudentScore
                    {
                        ServerId = local.ServerId,
                        AssessmentId = local.AssessmentId,
                        StudentId = local.StudentId,
                        Score = local.Score,
                        Status = Enum.TryParse<AttendanceStatus>(local.Status, out var s) ? s : AttendanceStatus.hadir
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get student scores: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    private bool IsInternetAvailable()
    {
        try
        {
            return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }
        catch
        {
            return false;
        }
    }
} 