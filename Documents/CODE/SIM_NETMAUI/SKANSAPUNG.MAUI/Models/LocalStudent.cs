using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalStudent
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; } // ID dari server (PostgreSQL), jika sudah sync
    public string Nis { get; set; }
    public string NamaLengkap { get; set; }
    public string Email { get; set; }
    public string Telp { get; set; }
    public string JenisKelamin { get; set; }
    public string Agama { get; set; }
    public long ClassRoomId { get; set; }
    public bool IsSynced { get; set; } // True jika sudah sync ke server
    public DateTime LastModified { get; set; }
} 