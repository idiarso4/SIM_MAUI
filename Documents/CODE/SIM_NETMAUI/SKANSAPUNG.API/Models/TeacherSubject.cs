using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class TeacherSubject
    {
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public long SubjectId { get; set; }
        public Subject Subject { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 