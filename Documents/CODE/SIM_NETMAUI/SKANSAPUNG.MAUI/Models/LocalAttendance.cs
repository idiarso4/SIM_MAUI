using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalAttendance
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; } // ID dari server (PostgreSQL), jika sudah sync
    public long UserId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public bool IsLeave { get; set; }
    public bool IsSynced { get; set; } // True jika sudah sync ke server
    public DateTime LastModified { get; set; }
} 