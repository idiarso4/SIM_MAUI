namespace SKANSAPUNG.API.Models;

public class Extracurricular
{
    public long Id { get; set; }
    public string Nama { get; set; }
    public string Deskripsi { get; set; }
    public string Hari { get; set; }
    public TimeSpan JamMulai { get; set; }
    public TimeSpan JamSelesai { get; set; }
    public string Tempat { get; set; }
    public bool Status { get; set; }
} 