namespace SKANSAPUNG.API.Models
{
    public class ScheduleItem
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DayOfWeek { get; set; }
        public string FormattedTime => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
} 