using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalExtracurricular
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; } // ID dari server (PostgreSQL), jika sudah sync
    public string Nama { get; set; }
    public string Deskripsi { get; set; }
    public string Hari { get; set; }
    public string JamMulai { get; set; } // TimeSpan as string
    public string JamSelesai { get; set; } // TimeSpan as string
    public string Tempat { get; set; }
    public bool Status { get; set; }
    public bool IsSynced { get; set; } // True jika sudah sync ke server
    public DateTime LastModified { get; set; }
} 