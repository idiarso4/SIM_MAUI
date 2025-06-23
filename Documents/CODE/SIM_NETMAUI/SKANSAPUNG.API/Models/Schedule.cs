using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Schedule
    {
        public long Id { get; set; }

        [Required]
        public long ClassRoomId { get; set; }

        [Required]
        public long SubjectId { get; set; }

        [Required]
        public long TeacherId { get; set; }
        
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }

        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ClassRoom ClassRoom { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
    }
} 