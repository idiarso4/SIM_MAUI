using SQLite;

namespace SKANSAPUNG.MAUI.Models
{
    [Table("schedule_items")]
    public class ScheduleItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public long ClassRoomId { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DayOfWeek { get; set; }

        [Ignore]
        public string FormattedTime => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
}