using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Assessment
    {
        public long Id { get; set; }
        
        [Required]
        public long ClassRoomId { get; set; }
        
        [Required]
        public long TeacherId { get; set; }
        
        [Required]
        public AssessmentType Type { get; set; }
        
        [Required]
        public string Subject { get; set; }
        
        [Required]
        public string AssessmentName { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public string Description { get; set; }
        
        public string Notes { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ClassRoom ClassRoom { get; set; }
        public User Teacher { get; set; }
        public List<StudentScore> StudentScores { get; set; } = new List<StudentScore>();
    }

    public enum AssessmentType
    {
        Sumatif,
        NonSumatif
    }
} 