using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class MyGradesViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IDatabaseService _databaseService;

        public class DisplayGrade
        {
            public string Subject { get; set; }
            public string AssessmentName { get; set; }
            public decimal Score { get; set; }
            public DateTime Date { get; set; }
        }

        [ObservableProperty]
        private ObservableCollection<DisplayGrade> _myGrades;

        public MyGradesViewModel(IConnectivityService connectivityService, IApiService apiService, IDatabaseService databaseService)
            : base(connectivityService)
        {
            Title = "Nilai Saya";
            _apiService = apiService;
            _databaseService = databaseService;
            MyGrades = new ObservableCollection<DisplayGrade>();
        }

        [RelayCommand]
        private async Task LoadMyGradesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // TODO: Ganti dengan ID pengguna yang login sesungguhnya
                long loggedInUserId = 1; 
                var student = await _databaseService.GetStudentByUserIdAsync(loggedInUserId);
                
                if (student == null && ConnectivityService.IsConnected)
                {
                    // Jika siswa tidak ada di lokal dan sedang online, coba sinkronisasi ulang data user.
                    var user = await _apiService.GetCurrentUserAsync(); // Asumsi user Id 1
                    var studentData = await _apiService.GetStudentAsync(user.Id); // Asumsi user id sama dengan student id
                    await _databaseService.SaveStudentAsync(new LocalStudent {
                        ServerId = studentData.Id.ToString(),
                        UserId = studentData.UserId.ToString(),
                        Name = studentData.NamaLengkap,
                        Email = studentData.Email,
                        ClassRoomId = studentData.ClassRoomId
                    });
                    student = await _databaseService.GetStudentByUserIdAsync(loggedInUserId);
                }

                if (student == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Data siswa tidak ditemukan.", "OK");
                    return;
                }

                if (ConnectivityService.IsConnected)
                {
                    var studentScores = await _apiService.GetStudentScoresAsync(long.Parse(student.ServerId));
                    await _databaseService.SaveStudentScoresAsync(studentScores);
                    
                    var assessments = await _apiService.GetAssessmentsAsync(student.ClassRoomId);
                    await _databaseService.SaveAssessmentsAsync(assessments);
                }

                var localScores = await _databaseService.GetStudentScoresAsync(long.Parse(student.ServerId));
                var localAssessments = await _databaseService.GetAssessmentsAsync();

                MyGrades.Clear();
                foreach (var score in localScores)
                {
                    var assessment = localAssessments.FirstOrDefault(a => a.ServerId == score.AssessmentId.ToString());
                    if (assessment != null)
                    {
                        MyGrades.Add(new DisplayGrade
                        {
                            Subject = assessment.Subject,
                            AssessmentName = assessment.AssessmentName,
                            Score = score.Score,
                            Date = assessment.Date
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading my grades: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Gagal memuat daftar nilai.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 