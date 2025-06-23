using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalAssessment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; }
    public long ClassRoomId { get; set; }
    public long TeacherId { get; set; }
    public string Type { get; set; } // assessment_type_enum as string
    public string Subject { get; set; }
    public string AssessmentName { get; set; }
    public string Date { get; set; } // Date as string
    public string Description { get; set; }
    public string Notes { get; set; }
    public bool IsSynced { get; set; }
    public DateTime LastModified { get; set; }
} 