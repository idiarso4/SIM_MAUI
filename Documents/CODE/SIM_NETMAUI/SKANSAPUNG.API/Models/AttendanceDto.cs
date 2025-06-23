namespace SKANSAPUNG.API.Models
{
    public class AttendanceDto
    {
        public long UserId { get; set; }
        public DateTime StartTime { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public DateTime? EndTime { get; set; }
        public double? EndLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public bool IsLeave { get; set; }
        public string? LocalId { get; set; } // ID dari database SQLite klien
    }
} 