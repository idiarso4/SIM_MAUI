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

                if (ClassRooms.Any())
                {
                    SelectedClassRoom = ClassRooms.First();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing schedule view: {ex.Message}");
                var localClasses = await _databaseService.GetClassRoomsAsync();
                ClassRooms.Clear();
                foreach (var c in localClasses)
                {
                    ClassRooms.Add(c);
                }
                if (ClassRooms.Any())
                {
                    SelectedClassRoom = ClassRooms.First();
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
                List<ScheduleItem> schedule = null;
                if (IsConnected)
                {
                    try
                    {
                        var apiSchedule = await _apiService.GetScheduleForClassAsync(SelectedClassRoom.Id);
                        if (apiSchedule != null && apiSchedule.Any())
                        {
                            var scheduleToSave = apiSchedule.Select(s => { s.ClassRoomId = SelectedClassRoom.Id; return s; }).ToList();
                            await _databaseService.SaveScheduleAsync(scheduleToSave);
                            schedule = scheduleToSave;
                        }
                    }
                    catch (Exception apiEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"API Error fetching schedule: {apiEx.Message}");
                    }
                }

                if (schedule == null)
                {
                    schedule = await _databaseService.GetScheduleForClassAsync(SelectedClassRoom.Id);
                }

                _fullSchedule = new List<ScheduleItem>(schedule ?? new List<ScheduleItem>());
                FilterSchedule();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading schedule: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Gagal memuat jadwal.", "OK");
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