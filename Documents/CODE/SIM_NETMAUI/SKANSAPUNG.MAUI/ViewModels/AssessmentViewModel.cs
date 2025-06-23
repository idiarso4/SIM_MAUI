using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class AssessmentViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<LocalAssessment> _assessments;

    [ObservableProperty]
    bool isRefreshing;

    public AssessmentViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        Title = "Assessments";
        Assessments = new ObservableCollection<LocalAssessment>();
    }

    [RelayCommand]
    public async Task GetAssessmentsAsync(long classRoomId)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            Assessments.Clear();
            if (IsInternetAvailable())
            {
                var apiList = await _apiService.GetAssessmentsAsync(classRoomId);
                foreach (var item in apiList)
                {
                    Assessments.Add(item);
                    // Simpan ke SQLite
                    var local = new LocalAssessment
                    {
                        ServerId = item.Id.ToString(),
                        ClassRoomId = item.ClassRoomId,
                        TeacherId = item.TeacherId,
                        Type = item.Type.ToString(),
                        Subject = item.Subject,
                        AssessmentName = item.AssessmentName,
                        Date = item.Date.ToString("yyyy-MM-dd"),
                        Description = item.Description,
                        Notes = item.Notes,
                        IsSynced = true,
                        LastModified = DateTime.UtcNow
                    };
                    await _databaseService.SaveLocalAssessmentAsync(local);
                }
            }
            else
            {
                // Offline: ambil dari SQLite
                var localList = await _databaseService.GetLocalAssessmentsAsync();
                foreach (var local in localList.Where(x => x.ClassRoomId == classRoomId))
                {
                    Assessments.Add(new LocalAssessment
                    {
                        ServerId = local.ServerId,
                        ClassRoomId = local.ClassRoomId,
                        TeacherId = local.TeacherId,
                        Type = Enum.TryParse<AssessmentType>(local.Type, out var t) ? t : AssessmentType.sumatif,
                        Subject = local.Subject,
                        AssessmentName = local.AssessmentName,
                        Date = DateTime.TryParse(local.Date, out var d) ? d : DateTime.MinValue,
                        Description = local.Description,
                        Notes = local.Notes
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get assessments: {ex.Message}");
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