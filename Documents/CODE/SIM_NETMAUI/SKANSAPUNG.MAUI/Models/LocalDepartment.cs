using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalDepartment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; }
    public string Name { get; set; }
    public string Kode { get; set; }
    public bool Status { get; set; }
    public bool IsSynced { get; set; }
    public DateTime LastModified { get; set; }
} 