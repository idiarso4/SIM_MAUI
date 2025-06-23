using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Department
    {
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Kode { get; set; }
        
        public bool Status { get; set; } = true;
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public List<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
    }
} 