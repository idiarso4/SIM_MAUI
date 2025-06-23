using SQLite;
using System;

namespace SKANSAPUNG.MAUI.Models
{
    public class LocalNotification
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime ReceivedAt { get; set; }
        public bool IsRead { get; set; }
    }
} 