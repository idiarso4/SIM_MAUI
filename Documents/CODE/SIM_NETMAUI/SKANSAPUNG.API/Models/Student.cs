using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Student
    {
        public long Id { get; set; }
        
        [Required]
        public string Nis { get; set; }
        
        [Required]
        public string NamaLengkap { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        public string Telp { get; set; }
        
        [Required]
        public Gender JenisKelamin { get; set; }
        
        [Required]
        public string Agama { get; set; }
        
        [Required]
        public long ClassRoomId { get; set; }
        
        public long? UserId { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ClassRoom ClassRoom { get; set; }
        public User User { get; set; }
        public List<StudentScore> Scores { get; set; } = new List<StudentScore>();
    }

    public enum Gender
    {
        L,
        P
    }
} 