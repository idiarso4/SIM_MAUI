using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKANSAPUNG.API.Models
{
    public class Shift
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [Column(TypeName = "time")]
        public TimeSpan StartTime { get; set; }
        
        [Column(TypeName = "time")]
        public TimeSpan EndTime { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? DeletedAt { get; set; }
    }
} 