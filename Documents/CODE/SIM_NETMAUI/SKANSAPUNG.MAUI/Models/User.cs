using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.MAUI.Models
{
    public class User
    {
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        public DateTime? EmailVerifiedAt { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string RememberToken { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public string Image { get; set; }
        
        [Required]
        public UserType UserType { get; set; } = UserType.Siswa;
        
        [Required]
        public Role Role { get; set; } = Role.Siswa;
        
        [Required]
        public Status Status { get; set; } = Status.Aktif;
        
        public long? ClassRoomId { get; set; }
        
        // Navigation properties
        public ClassRoom ClassRoom { get; set; }
        public StudentDetail StudentDetail { get; set; }
    }

    public enum UserType
    {
        Admin,
        Guru,
        Siswa
    }

    public enum Role
    {
        SuperAdmin,
        Admin,
        Guru,
        Siswa
    }

    public enum Status
    {
        Aktif,
        TidakAktif,
        Lulus,
        Pindah
    }
} 