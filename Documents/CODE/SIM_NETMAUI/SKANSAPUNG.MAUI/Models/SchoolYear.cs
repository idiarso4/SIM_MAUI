using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.MAUI.Models
{
    public class SchoolYear
    {
        public long Id { get; set; }
        
        [Required]
        public string Tahun { get; set; }
        
        [Required]
        public Semester Semester { get; set; }
        
        [Required]
        public SchoolYearStatus Status { get; set; } = SchoolYearStatus.Aktif;
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public List<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
    }

    public enum Semester
    {
        Ganjil,
        Genap
    }

    public enum SchoolYearStatus
    {
        Aktif,
        TidakAktif
    }
} 