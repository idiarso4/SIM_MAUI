using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKANSAPUNG.API.Models
{
    public class Office
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [Column(TypeName = "text")]
        public string Address { get; set; }
        
        [Column(TypeName = "double precision")]
        public double Latitude { get; set; }
        
        [Column(TypeName = "double precision")]
        public double Longitude { get; set; }
        
        public int Radius { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 