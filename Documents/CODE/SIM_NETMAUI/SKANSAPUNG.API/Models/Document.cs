using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKANSAPUNG.API.Models
{
    public class Document
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string FilePath { get; set; }
        
        [MaxLength(255)]
        public string FileType { get; set; }
        
        public int FileSize { get; set; }
        
        public long UploadedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public User Uploader { get; set; }
    }
} 