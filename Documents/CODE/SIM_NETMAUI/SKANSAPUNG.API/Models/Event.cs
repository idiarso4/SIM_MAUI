using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKANSAPUNG.API.Models
{
    public class Event
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        
        [Column(TypeName = "text")]
        public string Description { get; set; }
        
        [MaxLength(255)]
        public string Location { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public long OrganizerId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public User Organizer { get; set; }
    }
} 