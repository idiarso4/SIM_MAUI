using SKANSAPUNG.MAUI.Models;

namespace SKANSAPUNG.MAUI.Services
{
    public interface IApiService
    {
        // Authentication
        Task<User> LoginAsync(string username, string password);
        Task<bool> LogoutAsync();
    Task<IEnumerable<AttendanceRecord>> GetAttendanceDetailAsync(string studentId);
    Task<IEnumerable<GradeRecord>> GetGradesDetailAsync(string studentId, string subjectName);
        Task<User> GetCurrentUserAsync();
        Task<bool> RegisterUserAsync(string username, string password, string role);
        Task<bool> ValidateTokenAsync();
        Task<List<User>> GetUsersAsync(string userType);
        
        // Students
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(long id);
        Task<List<Student>> GetStudentsByClassRoomAsync(long classRoomId);
        Task<List<Student>> SearchStudentsAsync(string query);
        Task<List<string>> SyncStudentsAsync(IEnumerable<LocalStudent> studentRecords);
        Task<bool> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(long id);
        
        // Reports
        Task<ReportSummaryDto> GetReportSummaryAsync();
        Task<List<StudentAttendanceRecordDto>> GetAttendanceReportAsync(long classroomId, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<StudentGradeRecordDto>> GetGradesReportAsync(long classroomId, long? subjectId = null);
        Task<object> GetStudentReportAsync(long studentId);
        Task<object> GetClassroomReportAsync(long classroomId);
        Task<object> GetDepartmentReportAsync(long departmentId);
        Task<object> GetOverviewStatisticsAsync();
        Task<object> ExportAttendanceReportAsync(long classroomId, DateTime? startDate = null, DateTime? endDate = null);
        Task<object> ExportGradesReportAsync(long classroomId, long? subjectId = null);
        
        // ClassRooms
        Task<List<ClassRoom>> GetClassRoomsAsync();
        Task<ClassRoom> GetClassRoomAsync(long id);
        Task<List<ClassRoom>> GetClassRoomsByDepartmentAsync(long departmentId);
        Task<List<Student>> GetStudentsByClassRoomAsync(long classRoomId);
        Task<List<Schedule>> GetScheduleByClassRoomAsync(long classRoomId);
        Task<List<ClassRoom>> SearchClassRoomsAsync(string query);
        Task<bool> CreateClassRoomAsync(ClassRoom classRoom);
        Task<bool> UpdateClassRoomAsync(ClassRoom classRoom);
        Task<bool> DeleteClassRoomAsync(long id);
        Task<object> GetClassRoomStatisticsAsync(long id);
        
        // Departments
        Task<List<Department>> GetDepartmentsAsync();
        Task<Department> GetDepartmentAsync(long id);
        Task<List<ClassRoom>> GetClassesByDepartmentAsync(long departmentId);
        Task<List<Teacher>> GetTeachersByDepartmentAsync(long departmentId);
        Task<List<Subject>> GetSubjectsByDepartmentAsync(long departmentId);
        Task<List<Department>> SearchDepartmentsAsync(string query);
        Task<bool> CreateDepartmentAsync(Department department);
        Task<bool> UpdateDepartmentAsync(Department department);
        Task<bool> DeleteDepartmentAsync(long id);
        Task<object> GetDepartmentStatisticsAsync();
        
        // SchoolYears
        Task<List<SchoolYear>> GetSchoolYearsAsync();
        Task<SchoolYear> GetCurrentSchoolYearAsync();
        
        // Attendance
        Task<bool> CheckInAsync(Attendance attendance);
        Task<bool> CheckOutAsync(long attendanceId, double latitude, double longitude);
        Task<List<Attendance>> GetAttendanceHistoryAsync(long userId, DateTime startDate, DateTime endDate);
        Task<List<string>> SyncAttendanceAsync(IEnumerable<LocalAttendance> attendanceRecords);
        
        // Assessments
        Task<List<Assessment>> GetAssessmentsAsync(long classRoomId);
        Task<Assessment> GetAssessmentAsync(long id);
        Task<bool> CreateAssessmentAsync(Assessment assessment);
        
        // StudentScores (Grades)
        Task<List<StudentScore>> GetStudentScoresAsync(long studentId);
        Task<List<StudentScoreDetailDto>> GetScoresForAssessmentAsync(long assessmentId);
        Task<List<StudentScore>> GetScoresBySubjectAsync(long subjectId);
        Task<object> GetStudentGradeStatisticsAsync(long studentId);
        Task<object> GetClassroomGradeStatisticsAsync(long classroomId);
        Task<bool> SaveStudentScoreAsync(StudentScore score);
        Task<bool> UpdateScoresAsync(List<StudentScoreUpdateDto> scores);
        Task<bool> CreateBulkGradesAsync(List<StudentScore> grades);
        Task<List<string>> SyncGradesAsync(List<StudentScoreDetailDto> grades);
        Task<List<StudentScore>> SearchGradesAsync(string query);
        
        // Teachers
        Task<List<Teacher>> GetTeachersAsync();
        Task<Teacher> GetTeacherAsync(long id);
        Task<List<Teacher>> GetTeachersByDepartmentAsync(long departmentId);
        Task<List<Teacher>> SearchTeachersAsync(string query);
        Task<bool> CreateTeacherAsync(Teacher teacher);
        Task<bool> UpdateTeacherAsync(Teacher teacher);
        Task<bool> DeleteTeacherAsync(long id);
        
        // Subjects
        Task<List<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectAsync(long id);
        Task<List<Subject>> GetSubjectsByDepartmentAsync(long departmentId);
        Task<List<Teacher>> GetTeachersBySubjectAsync(long subjectId);
        Task<List<Subject>> SearchSubjectsAsync(string query);
        Task<bool> CreateSubjectAsync(Subject subject);
        Task<bool> UpdateSubjectAsync(Subject subject);
        Task<bool> DeleteSubjectAsync(long id);
        Task<bool> AssignTeacherToSubjectAsync(long teacherId, long subjectId);
        Task<bool> RemoveTeacherFromSubjectAsync(long teacherId, long subjectId);
        
        // Extracurriculars
        Task<List<Extracurricular>> GetExtracurricularsAsync();
        Task<Extracurricular> GetExtracurricularAsync(long id);
        Task<List<Student>> GetStudentsByExtracurricularAsync(long extracurricularId);
        Task<List<Extracurricular>> GetExtracurricularsByStudentAsync(long studentId);
        Task<List<Extracurricular>> SearchExtracurricularsAsync(string query);
        Task<bool> CreateExtracurricularAsync(Extracurricular extracurricular);
        Task<bool> UpdateExtracurricularAsync(Extracurricular extracurricular);
        Task<bool> DeleteExtracurricularAsync(long id);
        Task<bool> EnrollStudentAsync(long studentId, long extracurricularId);
        Task<bool> UnenrollStudentAsync(long studentId, long extracurricularId);
        Task<object> GetExtracurricularStatisticsAsync(long id);
        
        // Notifications
        Task<List<Notification>> GetNotificationsAsync();
        Task<Notification> GetNotificationAsync(long id);
        Task<List<Notification>> GetNotificationsByUserAsync(long userId);
        Task<bool> CreateNotificationAsync(Notification notification);
        Task<bool> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(long id);
        Task<bool> SendNotificationAsync(object notification);
        Task<bool> MarkNotificationAsReadAsync(long id);
        Task RegisterFcmTokenAsync(string token, string deviceId, string platform);
        Task UnregisterFcmTokenAsync(string deviceId);

        // Schedule
        Task<List<Schedule>> GetSchedulesAsync();
        Task<Schedule> GetScheduleAsync(long id);
        Task<List<Schedule>> GetSchedulesByClassAsync(long classId);
        Task<List<Schedule>> GetSchedulesByTeacherAsync(long teacherId);
        Task<List<Schedule>> GetSchedulesByDayAsync(string dayOfWeek);
        Task<bool> CreateScheduleAsync(Schedule schedule);
        Task<bool> UpdateScheduleAsync(Schedule schedule);
        Task<bool> DeleteScheduleAsync(long id);
    }
} 