using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalStudentScore
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; }
    public long AssessmentId { get; set; }
    public long StudentId { get; set; }
    public double? Score { get; set; }
    public string Status { get; set; } // attendance_status_enum as string
    public bool IsSynced { get; set; }
    public DateTime LastModified { get; set; }
} 