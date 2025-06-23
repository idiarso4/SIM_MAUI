using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKANSAPUNG.API.Models
{
    public class Announcement
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        
        [Column(TypeName = "text")]
        public string Content { get; set; }
        
        public long AuthorId { get; set; }
        
        public DateTime? PublishedAt { get; set; }
        
        public DateTime? ExpiresAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public User Author { get; set; }
    }
} 