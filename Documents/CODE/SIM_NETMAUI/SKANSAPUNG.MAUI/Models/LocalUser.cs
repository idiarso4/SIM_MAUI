using SQLite;

namespace SKANSAPUNG.MAUI.Models;

public class LocalUser
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ServerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string UserType { get; set; }
    public string Status { get; set; }
    public string Image { get; set; }
    public bool IsSynced { get; set; }
    public DateTime LastModified { get; set; }
} 