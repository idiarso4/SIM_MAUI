using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class GradesViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<ClassRoom> _classRooms;

        [ObservableProperty]
        private ObservableCollection<Assessment> _assessments;

        [ObservableProperty]
        private ObservableCollection<StudentScoreDetailDto> _studentScores;

        [ObservableProperty]
        private ClassRoom _selectedClassRoom;

        [ObservableProperty]
        private Assessment _selectedAssessment;

        public GradesViewModel(IDatabaseService databaseService, IApiService apiService, IConnectivityService connectivityService) : base(connectivityService)
        {
            _databaseService = databaseService;
            _apiService = apiService;
            Title = "Grades";
            ClassRooms = new ObservableCollection<ClassRoom>();
            Assessments = new ObservableCollection<Assessment>();
            StudentScores = new ObservableCollection<StudentScoreDetailDto>();
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var classes = await _apiService.GetClassRoomsAsync();
                if (classes != null && classes.Any())
                {
                    await _databaseService.ClearAndInsertAsync(classes);
                    ClassRooms.Clear();
                    foreach (var c in classes)
                    {
                        ClassRooms.Add(c);
                    }
                }
                else
                {
                    var localClasses = await _databaseService.GetClassRoomsAsync();
                    ClassRooms.Clear();
                    foreach (var c in localClasses)
                    {
                        ClassRooms.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching classrooms: {ex.Message}");
                var localClasses = await _databaseService.GetClassRoomsAsync();
                ClassRooms.Clear();
                foreach (var c in localClasses)
                {
                    ClassRooms.Add(c);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ClassRoomSelected()
        {
            if (SelectedClassRoom == null) return;
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Assessments.Clear();
                StudentScores.Clear();
                SelectedAssessment = null;

                var assessments = await _apiService.GetAssessmentsAsync(SelectedClassRoom.Id);
                if (assessments != null && assessments.Any())
                {
                    await _databaseService.ClearAndInsertAsync(assessments);
                    foreach (var a in assessments)
                    {
                        Assessments.Add(a);
                    }
                }
                else
                {
                    var localAssessments = await _databaseService.GetAssessmentsByClassAsync(SelectedClassRoom.Id);
                    foreach (var a in localAssessments)
                    {
                        Assessments.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching assessments: {ex.Message}");
                var localAssessments = await _databaseService.GetAssessmentsByClassAsync(SelectedClassRoom.Id);
                foreach (var a in localAssessments)
                {
                    Assessments.Add(a);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        [RelayCommand]
        private async Task SaveScoresAsync()
        {
            if (SelectedAssessment == null || !StudentScores.Any())
            {
                await Shell.Current.DisplayAlert("Gagal", "Pilih kelas dan penilaian terlebih dahulu, lalu muat data siswa.", "OK");
                return;
            }

            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var scoresToUpdate = StudentScores.Select(s => new StudentScoreUpdateDto
                {
                    StudentId = s.StudentId,
                    AssessmentId = SelectedAssessment.Id,
                    Score = s.Score
                }).ToList();

                var success = await _apiService.UpdateScoresAsync(scoresToUpdate);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Sukses", "Nilai berhasil disimpan.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Gagal", "Gagal menyimpan nilai. Silakan coba lagi.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($">>> Error saving scores: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Terjadi kesalahan saat menyimpan nilai.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AssessmentSelected()
        {
            if (SelectedAssessment == null) return;
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Selalu ambil dari API untuk data real-time,
                // karena nilai bisa sering berubah.
                var scores = await _apiService.GetScoresForAssessmentAsync(SelectedAssessment.Id);
                StudentScores.Clear();
                if (scores != null)
                {
                    foreach (var s in scores)
                    {
                        StudentScores.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching scores: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Gagal memuat data nilai siswa.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 