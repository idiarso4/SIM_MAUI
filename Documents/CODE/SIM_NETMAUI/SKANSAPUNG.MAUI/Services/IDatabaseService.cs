using SKANSAPUNG.MAUI.Models;

namespace SKANSAPUNG.MAUI.Services
{
    public interface IDatabaseService
    {
        // Local Student
        Task<List<LocalStudent>> GetStudentsAsync();
        Task<LocalStudent> GetStudentAsync(int id);
        Task<int> SaveStudentAsync(LocalStudent student);
        Task<int> DeleteStudentAsync(int id);
        Task<LocalStudent> GetStudentAsync(long id);
        Task<LocalStudent> GetStudentByUserIdAsync(long userId);
        Task<List<LocalStudent>> GetUnsyncedStudentsAsync();
        Task MarkStudentsAsSyncedAsync(IEnumerable<int> ids);
        Task<List<LocalDepartment>> GetDepartmentsAsync();
        Task SaveDepartmentsAsync(IEnumerable<Department> departments);
        Task<List<ScheduleItem>> GetScheduleForClassAsync(long classRoomId);
        Task SaveScheduleAsync(IEnumerable<ScheduleItem> schedule);

        // Local Attendance
        Task<List<LocalAttendance>> GetAttendanceHistoryAsync();
        Task<LocalAttendance> GetAttendanceAsync(int id);
        Task<int> SaveAttendanceAsync(LocalAttendance attendance);
        Task<int> DeleteAttendanceAsync(int id);
        Task<LocalAttendance> GetLastUnfinishedAttendanceAsync();
        Task<List<LocalAttendance>> GetUnsyncedAttendancesAsync();
        Task MarkAttendancesAsSyncedAsync(IEnumerable<int> ids);
        
        // Assessments
        Task<int> SaveAssessmentAsync(LocalAssessment assessment);
        Task<List<LocalAssessment>> GetAssessmentsAsync();
        Task<List<LocalAssessment>> GetAssessmentsByClassRoomAsync(long classRoomId);
        Task SaveAssessmentsAsync(IEnumerable<Assessment> assessments);

        // StudentScores
        Task<int> SaveStudentScoreAsync(LocalStudentScore score);
        Task<List<LocalStudentScore>> GetStudentScoresAsync(long studentId);
        Task SaveStudentScoresAsync(IEnumerable<StudentScore> scores);

        // Extracurriculars
        Task<int> SaveExtracurricularAsync(LocalExtracurricular extracurricular);
        
        // Sync
        Task<bool> SyncDataAsync();
        Task<bool> SyncStudentsAsync();
        Task<bool> SyncAttendanceAsync();

        // Local User
        Task<int> SaveUserAsync(LocalUser user);
        Task<LocalUser> GetUserAsync(long id);
        Task<List<LocalUser>> GetUsersAsync(string userType);
    }
    
    // Local database models
    public class LocalStudent
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsSynced { get; set; }
        public DateTime LastModified { get; set; }
    }
    
    public class LocalAttendance
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string ServerId { get; set; }
        public string UserId { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public DateTime StartTime { get; set; }
        public double? EndLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsSynced { get; set; }
        public DateTime LastModified { get; set; }
    }
} 