using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalClassRoom
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }
    public long DepartmentId { get; set; }
    public long SchoolYearId { get; set; }
    public long? HomeroomTeacherId { get; set; }
    public bool IsActive { get; set; }
    public bool IsSynced { get; set; }
    public DateTime LastModified { get; set; }
} 