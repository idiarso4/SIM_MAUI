namespace SKANSAPUNG.API.Models
{
    public class StudentScoreDetailDto
    {
        public long StudentId { get; set; }
        public string? StudentName { get; set; }
        public decimal? Score { get; set; }
        public string? Status { get; set; } // e.g., 'hadir', 'sakit', 'izin'
    }
} 