using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Teacher
    {
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public string Nip { get; set; }

        public string GelarDepan { get; set; }

        public string GelarBelakang { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public List<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    }
} 