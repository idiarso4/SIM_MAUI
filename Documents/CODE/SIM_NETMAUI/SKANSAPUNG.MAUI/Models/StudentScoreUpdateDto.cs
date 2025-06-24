namespace SKANSAPUNG.MAUI.Models;

public class StudentScoreUpdateDto
{
    public Guid StudentId { get; set; }
    public Guid AssessmentId { get; set; }
    public double? Score { get; set; }
}
