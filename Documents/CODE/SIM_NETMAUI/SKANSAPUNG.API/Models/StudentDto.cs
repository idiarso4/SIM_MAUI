namespace SKANSAPUNG.API.Models
{
    public class StudentDto
    {
        public long Id { get; set; }
        public string? NamaLengkap { get; set; }
        public string? Email { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string? JenisKelamin { get; set; }
        public string? Agama { get; set; }
        public string? Foto { get; set; }
        public bool IsSynced { get; set; }
        public string? LocalId { get; set; }
    }
} 