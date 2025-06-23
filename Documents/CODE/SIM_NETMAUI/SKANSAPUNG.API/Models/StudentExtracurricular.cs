using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class StudentExtracurricular
    {
        public long StudentId { get; set; }
        public Student Student { get; set; }

        public long ExtracurricularId { get; set; }
        public Extracurricular Extracurricular { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 