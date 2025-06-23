using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Permission
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string GuardName { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
