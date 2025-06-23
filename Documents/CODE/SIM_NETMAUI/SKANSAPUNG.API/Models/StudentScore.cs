using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class StudentScore
    {
        public long Id { get; set; }
        
        [Required]
        public long AssessmentId { get; set; }
        
        [Required]
        public long StudentId { get; set; }
        
        public decimal? Score { get; set; }
        
        [Required]
        public AttendanceStatus Status { get; set; } = AttendanceStatus.Hadir;
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public Assessment Assessment { get; set; }
        public Student Student { get; set; }
    }

    public enum AttendanceStatus
    {
        Hadir,
        Sakit,
        Izin,
        Alpha
    }
} 