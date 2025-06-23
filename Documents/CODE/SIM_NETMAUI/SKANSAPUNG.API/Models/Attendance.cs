using System.ComponentModel.DataAnnotations;

namespace SKANSAPUNG.API.Models
{
    public class Attendance
    {
        public long Id { get; set; }
        
        [Required]
        public long UserId { get; set; }
        
        [Required]
        public double ScheduleLatitude { get; set; }
        
        [Required]
        public double ScheduleLongitude { get; set; }
        
        [Required]
        public TimeSpan ScheduleStartTime { get; set; }
        
        [Required]
        public TimeSpan ScheduleEndTime { get; set; }
        
        [Required]
        public double StartLatitude { get; set; }
        
        [Required]
        public double StartLongitude { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        public bool IsLeave { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? DeletedAt { get; set; }
        
        public double? EndLatitude { get; set; }
        
        public double? EndLongitude { get; set; }
        
        // Navigation properties
        public User User { get; set; }
    }
} 