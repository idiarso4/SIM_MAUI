namespace SKANSAPUNG.MAUI.Models;

public class StudentAttendanceRecordDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } // e.g., "Hadir", "Izin", "Sakit", "Alpa"
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
}
