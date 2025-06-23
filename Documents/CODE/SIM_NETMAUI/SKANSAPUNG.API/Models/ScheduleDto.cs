namespace SKANSAPUNG.API.Models
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? DayOfWeek { get; set; } // e.g., "Senin", "Selasa"
    }
} 