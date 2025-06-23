using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.ViewModels
{
    public partial class ScheduleViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IApiService _apiService;
        private List<ScheduleItem> _fullSchedule;

        [ObservableProperty]
        private ObservableCollection<ClassRoom> _classRooms;

        [ObservableProperty]
        private ClassRoom _selectedClassRoom;

        [ObservableProperty]
        private ObservableCollection<string> _days;

        [ObservableProperty]
        private string _selectedDay;

        [ObservableProperty]
        private ObservableCollection<ScheduleItem> _dailySchedule;

        public ScheduleViewModel(IDatabaseService databaseService, IApiService apiService, IConnectivityService connectivityService) : base(connectivityService)
        {
            _databaseService = databaseService;
            _apiService = apiService;
            Title = "Schedule";
            _fullSchedule = new List<ScheduleItem>();
            ClassRooms = new ObservableCollection<ClassRoom>();
            Days = new ObservableCollection<string> { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            DailySchedule = new ObservableCollection<ScheduleItem>();
            SelectedDay = "Senin";
        }

        public async Task InitializeAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var classes = await _databaseService.GetClassRoomsAsync();
                if (classes.Any())
                {
                    ClassRooms.Clear();
                    foreach (var c in classes)
                    {
                        ClassRooms.Add(c);
                    }
                    SelectedClassRoom = ClassRooms.First();
                    await LoadSchedule();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoadSchedule()
        {
            if (SelectedClassRoom == null || IsBusy) return;

            IsBusy = true;
            try
            {
                // Load from local database first
                var localSchedule = await _databaseService.GetScheduleForClassAsync(SelectedClassRoom.Id);
                _fullSchedule = new List<ScheduleItem>(localSchedule);
                FilterSchedule();

                // Fetch from API if connected
                if (IsConnected)
                {
                    var apiSchedule = await _apiService.GetScheduleForClassAsync(SelectedClassRoom.Id);
                    if (apiSchedule != null && apiSchedule.Any())
                    {
                        // Assign ClassRoomId to each item before saving
                        var scheduleToSave = apiSchedule.Select(s => { s.ClassRoomId = SelectedClassRoom.Id; return s; }).ToList();
                        await _databaseService.SaveScheduleAsync(scheduleToSave);
                        _fullSchedule = new List<ScheduleItem>(scheduleToSave);
                        FilterSchedule();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                Console.WriteLine($"Error loading schedule: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void FilterSchedule()
        {
            DailySchedule.Clear();
            if (_fullSchedule == null || string.IsNullOrEmpty(SelectedDay)) return;

            var filtered = _fullSchedule.Where(s => s.DayOfWeek.Equals(SelectedDay, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach(var item in filtered)
            {
                DailySchedule.Add(item);
            }
        }

        partial void OnSelectedClassRoomChanged(ClassRoom value)
        {
            if (value != null)
                LoadScheduleCommand.Execute(null);
        }

        partial void OnSelectedDayChanged(string value)
        {
            FilterSchedule();
        }

        [RelayCommand]
        private void SelectDay(string day)
        {
            if (!string.IsNullOrEmpty(day))
            {
                SelectedDay = day;
            }
        }
    }
} 