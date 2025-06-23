using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class ClassRoom
    {
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Level { get; set; }
        
        [Required]
        public long DepartmentId { get; set; }
        
        [Required]
        public long SchoolYearId { get; set; }
        
        public long? HomeroomTeacherId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public Department Department { get; set; }
        public SchoolYear SchoolYear { get; set; }
        public User HomeroomTeacher { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
} 