using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
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

        public async Task InitializeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var classes = await _databaseService.GetClassRoomsAsync();
                ClassRooms.Clear();
                foreach (var c in classes)
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
                var assessments = await _databaseService.GetAssessmentsByClassAsync(SelectedClassRoom.Id);
                Assessments.Clear();
                StudentScores.Clear();
                SelectedAssessment = null;
                foreach (var a in assessments)
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
                foreach (var s in scores)
                {
                    StudentScores.Add(s);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 