namespace SKANSAPUNG.MAUI.Models;

public class StudentGradeRecordDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; }
    public string SubjectName { get; set; }
    public string AssessmentName { get; set; }
    public double Score { get; set; }
    public string Grade { get; set; } // e.g., A, B, C
}
