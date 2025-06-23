using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKANSAPUNG.API.Models
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Type { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string NotifiableType { get; set; }
        
        public long NotifiableId { get; set; }
        
        [Column(TypeName = "jsonb")]
        public string Data { get; set; }
        
        public DateTime? ReadAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 