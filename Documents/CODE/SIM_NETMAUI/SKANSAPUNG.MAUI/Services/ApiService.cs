using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SKANSAPUNG.MAUI.Models;
using SKANSAPUNG.MAUI.Models.Configuration;

namespace SKANSAPUNG.MAUI.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly string _baseUrl;

        public ApiService(IAuthService authService, ApiSettings apiSettings)
        {
            _authService = authService;
            _baseUrl = apiSettings.BaseUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task SetAuthHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = !string.IsNullOrEmpty(token)
                ? new AuthenticationHeaderValue("Bearer", token)
                : null;
        }

        // Generic HTTP request helpers
        private async Task<T> GetAsync<T>(string endpoint)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
            if (!response.IsSuccessStatusCode) return default;
            try
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON Deserialization Error: {ex.Message}");
                return default;
            }
        }

        private async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            await SetAuthHeaderAsync();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);
            return response.IsSuccessStatusCode;
        }

        private async Task<TResponse> PostAsync<TResponse, TRequest>(string endpoint, TRequest data)
        {
            await SetAuthHeaderAsync();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<TResponse>() : default;
        }

        private async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            await SetAuthHeaderAsync();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}{endpoint}", content);
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> DeleteAsync(string endpoint)
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}");
            return response.IsSuccessStatusCode;
        }

        // Authentication
        public async Task<User> LoginAsync(string email, string password)
        {
            // This is handled by AuthService
            throw new NotImplementedException();
        }
        
        public async Task<bool> LogoutAsync()
        {
            // This is handled by AuthService
            throw new NotImplementedException();
        }
        
        public Task<User> GetCurrentUserAsync() => GetAsync<User>("/user");

        public async Task<List<User>> GetUsersAsync(string userType)
        {
            var users = await GetAsync<List<User>>($"/users?type={userType}");
            return users ?? new List<User>();
        }

        public async Task<List<string>> SyncStudentsAsync(IEnumerable<LocalStudent> studentRecords)
        {
            if (!studentRecords.Any())
                return new List<string>();

            var dtoList = studentRecords.Select(s => new
            {
                s.ServerId, s.Name, s.Email, s.Telp, s.JenisKelamin, s.Agama, s.ClassRoomId, s.UserId, LocalId = s.Id.ToString()
            }).ToList();

            var result = await PostAsync<JsonElement, object>("/students/sync", dtoList);
            if (result.TryGetProperty("syncedIds", out var syncedIdsProperty))
            {
                return syncedIdsProperty.Deserialize<List<string>>() ?? new List<string>();
            }
            return new List<string>();
        }

        // Students
        public async Task<List<Student>> GetStudentsAsync()
        {
            var students = await GetAsync<List<Student>>("/students");
            return students ?? new List<Student>();
        }

        public Task<Student> GetStudentAsync(long id) => GetAsync<Student>($"/students/{id}");

        public Task<bool> CreateStudentAsync(Student student) => PostAsync("/students", student);

        public Task<bool> UpdateStudentAsync(Student student) => PutAsync($"/students/{student.Id}", student);

        public Task<bool> DeleteStudentAsync(long id) => DeleteAsync($"/students/{id}");
        
        // ClassRooms
        public async Task<List<ClassRoom>> GetClassRoomsAsync()
        {
            var classRooms = await GetAsync<List<ClassRoom>>("/classrooms");
            return classRooms ?? new List<ClassRoom>();
        }

        public Task<ClassRoom> GetClassRoomAsync(long id) => GetAsync<ClassRoom>($"/classrooms/{id}");

        // Departments
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            var departments = await GetAsync<List<Department>>("/departments");
            return departments ?? new List<Department>();
        }

        // SchoolYears
        public async Task<List<SchoolYear>> GetSchoolYearsAsync()
        {
            var schoolYears = await GetAsync<List<SchoolYear>>("/schoolyears");
            return schoolYears ?? new List<SchoolYear>();
        }

        public Task<SchoolYear> GetCurrentSchoolYearAsync() => GetAsync<SchoolYear>("/schoolyears/current");
        
        // Attendance
        public Task<bool> CheckInAsync(Attendance attendance) => PostAsync("/attendance/checkin", attendance);

        public Task<bool> CheckOutAsync(long attendanceId, double latitude, double longitude)
        {
            var checkoutData = new { AttendanceId = attendanceId, Latitude = latitude, Longitude = longitude };
            return PostAsync("/attendance/checkout", checkoutData);
        }

        public async Task<List<Attendance>> GetAttendanceHistoryAsync(long userId, DateTime startDate, DateTime endDate)
        {
            var history = await GetAsync<List<Attendance>>($"/attendance/history?userId={userId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return history ?? new List<Attendance>();
        }

        public async Task<List<string>> SyncAttendanceAsync(IEnumerable<LocalAttendance> attendanceRecords)
        {
            if (!attendanceRecords.Any()) return new List<string>();

            var dtoList = attendanceRecords.Select(a => new
            {
                a.UserId, a.StartTime, a.StartLatitude, a.StartLongitude, a.EndTime, a.EndLatitude, a.EndLongitude, a.IsLeave, LocalId = a.Id.ToString()
            }).ToList();

            var result = await PostAsync<JsonElement, object>("/attendance/sync", dtoList);
            if (result.TryGetProperty("syncedIds", out var syncedIdsProperty))
            {
                return syncedIdsProperty.Deserialize<List<string>>() ?? new List<string>();
            }
            return new List<string>();
        }

        // Assessments
        public async Task<List<Assessment>> GetAssessmentsAsync(long classRoomId)
        {
            var assessments = await GetAsync<List<Assessment>>($"/assessments?classRoomId={classRoomId}");
            return assessments ?? new List<Assessment>();
        }

        public Task<Assessment> GetAssessmentAsync(long id) => GetAsync<Assessment>($"/assessments/{id}");

        // StudentScores
        public async Task<List<StudentScore>> GetStudentScoresAsync(long studentId)
        {
            var scores = await GetAsync<List<StudentScore>>($"/studentscores?studentId={studentId}");
            return scores ?? new List<StudentScore>();
        }

        public async Task<List<StudentScoreDetailDto>> GetScoresForAssessmentAsync(long assessmentId)
        {
            var scores = await GetAsync<List<StudentScoreDetailDto>>($"/grades/assessment/{assessmentId}");
            return scores ?? new List<StudentScoreDetailDto>();
        }

        public Task<bool> SaveStudentScoreAsync(StudentScore score)
        {
            return score.Id > 0
                ? PutAsync($"/studentscores/{score.Id}", score)
                : PostAsync("/studentscores", score);
        }

        // Extracurriculars
        public async Task<List<Extracurricular>> GetExtracurricularsAsync()
        {
            var extracurriculars = await GetAsync<List<Extracurricular>>("/extracurriculars");
            return extracurriculars ?? new List<Extracurricular>();
        }

        // Notifications
        public Task<bool> SendNotificationAsync(object notification) => PostAsync("/notifications/send", notification);

        public async Task RegisterFcmTokenAsync(string token, string deviceId, string platform)
        {
            try
            {
                var payload = new { Token = token, DeviceId = deviceId, Platform = platform };
                await PostAsync("/notifications/register-token", payload);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception on RegisterFcmTokenAsync: {ex.Message}");
            }
        }

        // Reports
        public Task<ReportSummaryDto> GetReportSummaryAsync() => GetAsync<ReportSummaryDto>("/reports/summary");

        public async Task<List<ScheduleItem>> GetScheduleForClassAsync(long classId)
        {
            var schedule = await GetAsync<List<ScheduleItem>>($"/schedule/{classId}");
            return schedule ?? new List<ScheduleItem>();
        }

        // Teachers
        public async Task<List<Teacher>> GetTeachersAsync()
        {
            var teachers = await GetAsync<List<Teacher>>("/teachers");
            return teachers ?? new List<Teacher>();
        }

        public Task<Teacher> GetTeacherAsync(long id) => GetAsync<Teacher>($"/teachers/{id}");

        public async Task<List<Teacher>> GetTeachersByDepartmentAsync(long departmentId)
        {
            var teachers = await GetAsync<List<Teacher>>($"/teachers/department/{departmentId}");
            return teachers ?? new List<Teacher>();
        }

        public async Task<List<Teacher>> SearchTeachersAsync(string query)
        {
            var teachers = await GetAsync<List<Teacher>>($"/teachers/search?query={query}");
            return teachers ?? new List<Teacher>();
        }

        public Task<bool> CreateTeacherAsync(Teacher teacher) => PostAsync("/teachers", teacher);

        public Task<bool> UpdateTeacherAsync(Teacher teacher) => PutAsync($"/teachers/{teacher.Id}", teacher);

        public Task<bool> DeleteTeacherAsync(long id) => DeleteAsync($"/teachers/{id}");

        // Subjects
        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = await GetAsync<List<Subject>>("/subjects");
            return subjects ?? new List<Subject>();
        }

        public Task<Subject> GetSubjectAsync(long id) => GetAsync<Subject>($"/subjects/{id}");

        public async Task<List<Subject>> GetSubjectsByDepartmentAsync(long departmentId)
        {
            var subjects = await GetAsync<List<Subject>>($"/subjects/department/{departmentId}");
            return subjects ?? new List<Subject>();
        }

        public async Task<List<Teacher>> GetTeachersBySubjectAsync(long subjectId)
        {
            var teachers = await GetAsync<List<Teacher>>($"/subjects/{subjectId}/teachers");
            return teachers ?? new List<Teacher>();
        }

        public async Task<List<Subject>> SearchSubjectsAsync(string query)
        {
            var subjects = await GetAsync<List<Subject>>($"/subjects/search?query={query}");
            return subjects ?? new List<Subject>();
        }

        public Task<bool> CreateSubjectAsync(Subject subject) => PostAsync("/subjects", subject);

        public Task<bool> UpdateSubjectAsync(Subject subject) => PutAsync($"/subjects/{subject.Id}", subject);

        public Task<bool> DeleteSubjectAsync(long id) => DeleteAsync($"/subjects/{id}");

        public Task<bool> AssignTeacherToSubjectAsync(long teacherId, long subjectId) => PostAsync($"/subjects/{subjectId}/assign-teacher", new { TeacherId = teacherId });

        public Task<bool> RemoveTeacherFromSubjectAsync(long teacherId, long subjectId) => PostAsync($"/subjects/{subjectId}/remove-teacher", new { TeacherId = teacherId });

        public Task<bool> RegisterUserAsync(string username, string password, string role)
        {
            var registerData = new { Username = username, Password = password, Role = role };
            return PostAsync("/auth/register", registerData);
        }

        public async Task<bool> ValidateTokenAsync()
        {
            var response = await GetAsync<object>("/auth/validate");
            return response != null;
        }

        public async Task<List<Student>> GetStudentsByClassRoomAsync(long classRoomId)
        {
            var students = await GetAsync<List<Student>>($"/students/classroom/{classRoomId}");
            return students ?? new List<Student>();
        }

        public async Task<List<Student>> SearchStudentsAsync(string query)
        {
            var students = await GetAsync<List<Student>>($"/students/search?query={query}");
            return students ?? new List<Student>();
        }

        // ClassRooms (lanjutan)
        public async Task<List<ClassRoom>> GetClassRoomsByDepartmentAsync(long departmentId)
        {
            var classrooms = await GetAsync<List<ClassRoom>>($"/classrooms/department/{departmentId}");
            return classrooms ?? new List<ClassRoom>();
        }

        public async Task<List<Schedule>> GetScheduleByClassRoomAsync(long classRoomId)
        {
            var schedules = await GetAsync<List<Schedule>>($"/schedule/classroom/{classRoomId}");
            return schedules ?? new List<Schedule>();
        }

        public async Task<List<ClassRoom>> SearchClassRoomsAsync(string query)
        {
            var classrooms = await GetAsync<List<ClassRoom>>($"/classrooms/search?query={query}");
            return classrooms ?? new List<ClassRoom>();
        }

        public Task<bool> CreateClassRoomAsync(ClassRoom classRoom) => PostAsync("/classrooms", classRoom);

        public Task<bool> UpdateClassRoomAsync(ClassRoom classRoom) => PutAsync($"/classrooms/{classRoom.Id}", classRoom);

        public Task<bool> DeleteClassRoomAsync(long id) => DeleteAsync($"/classrooms/{id}");

        public Task<object> GetClassRoomStatisticsAsync(long id) => GetAsync<object>($"/classrooms/{id}/statistics");

        // Departments (lanjutan)
        public Task<Department> GetDepartmentAsync(long id) => GetAsync<Department>($"/departments/{id}");

        public async Task<List<ClassRoom>> GetClassesByDepartmentAsync(long departmentId)
        {
            // This seems redundant with GetClassRoomsByDepartmentAsync, but implementing as per interface
            var classrooms = await GetAsync<List<ClassRoom>>($"/departments/{departmentId}/classes");
            return classrooms ?? new List<ClassRoom>();
        }

        public async Task<List<Department>> SearchDepartmentsAsync(string query)
        {
            var departments = await GetAsync<List<Department>>($"/departments/search?query={query}");
            return departments ?? new List<Department>();
        }

        public Task<bool> CreateDepartmentAsync(Department department) => PostAsync("/departments", department);

        public Task<bool> UpdateDepartmentAsync(Department department) => PutAsync($"/departments/{department.Id}", department);

        public Task<bool> DeleteDepartmentAsync(long id) => DeleteAsync($"/departments/{id}");

        public Task<object> GetDepartmentStatisticsAsync() => GetAsync<object>("/departments/statistics");

        // Assessments (lanjutan)
        public Task<bool> CreateAssessmentAsync(Assessment assessment) => PostAsync("/assessments", assessment);

        // StudentScores (Grades) (lanjutan)
        public async Task<List<StudentScore>> GetScoresBySubjectAsync(long subjectId)
        {
            var scores = await GetAsync<List<StudentScore>>($"/grades/subject/{subjectId}");
            return scores ?? new List<StudentScore>();
        }

        public Task<object> GetStudentGradeStatisticsAsync(long studentId) => GetAsync<object>($"/grades/statistics/student/{studentId}");

        public Task<object> GetClassroomGradeStatisticsAsync(long classroomId) => GetAsync<object>($"/grades/statistics/classroom/{classroomId}");

        public Task<bool> CreateBulkGradesAsync(List<StudentScore> grades) => PostAsync("/grades/bulk", grades);

        public async Task<List<string>> SyncGradesAsync(List<StudentScoreDetailDto> grades)
        {
            var result = await PostAsync<JsonElement, object>("/grades/sync", grades);
            if (result.TryGetProperty("syncedIds", out var syncedIdsProperty))
            {
                return syncedIdsProperty.Deserialize<List<string>>() ?? new List<string>();
            }
            return new List<string>();
        }

        public async Task<List<StudentScore>> SearchGradesAsync(string query)
        {
            var grades = await GetAsync<List<StudentScore>>($"/grades/search?query={query}");
            return grades ?? new List<StudentScore>();
        }

        // Attendance (lanjutan)
        public async Task<List<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var attendances = await GetAsync<List<Attendance>>($"/attendance/daterange?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return attendances ?? new List<Attendance>();
        }

        public Task<object> GetAttendanceStatisticsAsync() => GetAsync<object>("/attendance/statistics");

        // Extracurricular (lanjutan)
        public Task<bool> EnrollStudentInExtracurricularAsync(long studentId, long extracurricularId) => PostAsync($"/extracurriculars/{extracurricularId}/enroll", new { StudentId = studentId });

        public Task<bool> RemoveStudentFromExtracurricularAsync(long studentId, long extracurricularId) => PostAsync($"/extracurriculars/{extracurricularId}/remove", new { StudentId = studentId });

        public async Task<List<Student>> GetStudentsByExtracurricularAsync(long extracurricularId)
        {
            var students = await GetAsync<List<Student>>($"/extracurriculars/{extracurricularId}/students");
            return students ?? new List<Student>();
        }

        // Notifications (lanjutan)
        public Task<bool> MarkAllNotificationsAsReadAsync() => PostAsync("/notifications/mark-all-as-read", new { });

        public Task<bool> DeleteNotificationAsync(long id) => DeleteAsync($"/notifications/{id}");

        public Task<bool> CreateNotificationAsync(Notification notification) => PostAsync("/notifications", notification);

        public async Task<int> GetUnreadNotificationCountAsync()
        {
            var result = await GetAsync<JsonElement>("/notifications/unread-count");
            if (result.TryGetProperty("count", out var countProperty))
            {
                return countProperty.GetInt32();
            }
            return 0;
        }

        // Reports
        public async Task<List<Report>> GetReportsAsync()
        {
            var reports = await GetAsync<List<Report>>("/reports");
            return reports ?? new List<Report>();
        }

        public Task<Report> GetReportAsync(long id) => GetAsync<Report>($"/reports/{id}");

        public Task<bool> CreateReportAsync(Report report) => PostAsync("/reports", report);

        public Task<bool> UpdateReportAsync(Report report) => PutAsync($"/reports/{report.Id}", report);

        public Task<bool> DeleteReportAsync(long id) => DeleteAsync($"/reports/{id}");

        public async Task<List<Report>> GetReportsByStudentAsync(long studentId)
        {
            var reports = await GetAsync<List<Report>>($"/reports/student/{studentId}");
            return reports ?? new List<Report>();
        }

        public async Task<List<Report>> GetReportsByTeacherAsync(long teacherId)
        {
            var reports = await GetAsync<List<Report>>($"/reports/teacher/{teacherId}");
            return reports ?? new List<Report>();
        }

        public Task<object> GetSystemStatisticsAsync() => GetAsync<object>("/statistics/system");
    }
} 