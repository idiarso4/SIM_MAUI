using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace SKANSAPUNG.MAUI.ViewModels;

public partial class StudentsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<LocalStudent> _students;

    [ObservableProperty]
    private string searchText;

    [ObservableProperty]
    private Student selectedStudent;

    [ObservableProperty]
    private bool isSearchVisible;

    [ObservableProperty]
    private ObservableCollection<ClassRoom> classRooms;

    [ObservableProperty]
    private ClassRoom selectedClassRoom;

    public StudentsViewModel(IApiService apiService, IDatabaseService databaseService, IConnectivityService connectivityService)
        : base(connectivityService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        Title = "Students";
        Students = new ObservableCollection<LocalStudent>();
        ClassRooms = new ObservableCollection<ClassRoom>();
    }

    public async Task InitializeAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Load class rooms for filtering
            await LoadClassRoomsAsync();

            // Load students
            await LoadStudentsAsync();
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Failed to load data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadClassRoomsAsync()
    {
        try
        {
            var classRoomsList = await _apiService.GetClassRoomsAsync();
            ClassRooms.Clear();
            foreach (var classRoom in classRoomsList)
            {
                ClassRooms.Add(classRoom);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading class rooms: {ex.Message}");
        }
    }

    private async Task LoadStudentsAsync()
    {
        try
        {
            Students.Clear();
            if (IsInternetAvailable())
            {
                var studentsList = await _apiService.GetStudentsAsync();
                // Simpan ke SQLite
                foreach (var student in studentsList)
                {
                    Students.Add(student);
                    // Simpan ke SQLite (mapping ke LocalStudent)
                    var local = new LocalStudent
                    {
                        ServerId = student.Id.ToString(),
                        Nis = student.Nis,
                        NamaLengkap = student.NamaLengkap,
                        Email = student.Email,
                        Telp = student.Telp,
                        JenisKelamin = student.JenisKelamin.ToString(),
                        Agama = student.Agama,
                        ClassRoomId = student.ClassRoomId,
                        IsSynced = true,
                        LastModified = DateTime.UtcNow
                    };
                    await _databaseService.SaveLocalStudentAsync(local);
                }
            }
            else
            {
                // Offline: ambil dari SQLite
                var localStudents = await _databaseService.GetLocalStudentsAsync();
                foreach (var local in localStudents)
                {
                    Students.Add(new Student
                    {
                        Id = long.TryParse(local.ServerId, out var sid) ? sid : 0,
                        Nis = local.Nis,
                        NamaLengkap = local.NamaLengkap,
                        Email = local.Email,
                        Telp = local.Telp,
                        JenisKelamin = Enum.TryParse<Gender>(local.JenisKelamin, out var g) ? g : Gender.L,
                        Agama = local.Agama,
                        ClassRoomId = local.ClassRoomId
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Failed to load students: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SearchStudentsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var searchTerm = SearchText?.Trim();
            var classRoomId = SelectedClassRoom?.Id;

            var filteredStudents = await _apiService.SearchStudentsAsync(searchTerm, classRoomId);
            
            Students.Clear();
            foreach (var student in filteredStudents)
            {
                Students.Add(student);
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Search failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddStudentAsync()
    {
        await Shell.Current.GoToAsync("AddStudent");
    }

    [RelayCommand]
    private async Task EditStudentAsync(Student student)
    {
        if (student == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "StudentId", student.Id }
        };

        await Shell.Current.GoToAsync("EditStudent", parameters);
    }

    [RelayCommand]
    private async Task ViewStudentDetailAsync(Student student)
    {
        if (student == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "StudentId", student.Id }
        };

        await Shell.Current.GoToAsync("StudentDetail", parameters);
    }

    [RelayCommand]
    private async Task DeleteStudentAsync(Student student)
    {
        if (student == null) return;

        var confirmed = await ShowConfirmationAsync(
            "Delete Student", 
            $"Are you sure you want to delete {student.NamaLengkap}?",
            "Delete",
            "Cancel"
        );

        if (confirmed)
        {
            try
            {
                IsBusy = true;
                var result = await _apiService.DeleteStudentAsync(student.Id);
                
                if (result.IsSuccess)
                {
                    Students.Remove(student);
                    await ShowAlertAsync("Success", "Student deleted successfully.");
                }
                else
                {
                    await ShowAlertAsync("Error", result.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Error", $"Failed to delete student: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    [RelayCommand]
    private void ToggleSearch()
    {
        IsSearchVisible = !IsSearchVisible;
        if (!IsSearchVisible)
        {
            SearchText = string.Empty;
            SelectedClassRoom = null;
            _ = LoadStudentsAsync();
        }
    }

    [RelayCommand]
    private async Task RefreshStudentsAsync()
    {
        await LoadStudentsAsync();
    }

    partial void OnSelectedClassRoomChanged(ClassRoom value)
    {
        if (value != null)
        {
            _ = SearchStudentsAsync();
        }
    }

    protected override async Task OnRefreshAsync()
    {
        await LoadDataAsync();
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

    [RelayCommand]
    private async Task GoToDetails(LocalStudent student)
    {
        if (student == null)
            return;

        await Shell.Current.GoToAsync(nameof(StudentDetailPage), true, new Dictionary<string, object>
        {
            {"Student", student }
        });
    }

    [RelayCommand]
    private async Task SyncStudentsAsync()
    {
        if (IsBusy) return;

        if (!ConnectivityService.IsConnected)
        {
            await Shell.Current.DisplayAlert("Offline", "Tidak ada koneksi internet. Sinkronisasi akan dicoba lagi nanti.", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            var unsyncedStudents = await _databaseService.GetUnsyncedStudentsAsync();
            if (!unsyncedStudents.Any())
            {
                await Shell.Current.DisplayAlert("Sync", "Tidak ada data siswa untuk disinkronkan.", "OK");
                return;
            }

            var syncedIds = await _apiService.SyncStudentsAsync(unsyncedStudents);
            if (syncedIds != null && syncedIds.Any())
            {
                await _databaseService.MarkStudentsAsSyncedAsync(syncedIds.Select(int.Parse));
                await Shell.Current.DisplayAlert("Success", $"{syncedIds.Count} data siswa berhasil disinkronkan.", "OK");
                await LoadStudentsAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Sync Failed", "Gagal menyinkronkan data siswa.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error syncing students: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Terjadi kesalahan saat sinkronisasi.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
} 