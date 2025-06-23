using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Gallery
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 