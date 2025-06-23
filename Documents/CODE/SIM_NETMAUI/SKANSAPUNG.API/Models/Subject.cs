using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Subject
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 