using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SKANSAPUNG.MAUI.Models;

namespace SKANSAPUNG.MAUI.Services
{
    /// <summary>
    /// Extension methods for ApiService to implement the new controller endpoints.
    /// Add these methods to your ApiService class.
    /// </summary>
    public static class ApiServiceExtensions
    {
        // Authentication methods
        public static async Task<User> LoginAsync(this ApiService service, HttpClient httpClient, string baseUrl, IAuthService authService, string username, string password)
        {
            var loginData = new { Username = username, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{baseUrl}/auth/login", content);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                if (result.TryGetProperty("token", out var tokenProperty) && 
                    result.TryGetProperty("user", out var userProperty))
                {
                    var token = tokenProperty.GetString();
                    await authService.SaveTokenAsync(token);
                    return userProperty.Deserialize<User>();
                }
            }
            
            return null;
        }
        
        public static async Task<bool> RegisterUserAsync(this ApiService service, HttpClient httpClient, string baseUrl, string username, string password, string role)
        {
            var registerData = new { Username = username, Password = password, Role = role };
            var content = new StringContent(JsonSerializer.Serialize(registerData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{baseUrl}/auth/register", content);
            
            return response.IsSuccessStatusCode;
        }
        
        public static async Task<bool> ValidateTokenAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/auth/validate");
            return response.IsSuccessStatusCode;
        }
        
        // Teacher methods
        public static async Task<List<Teacher>> GetTeachersAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/teachers");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Teacher>>();
            }
            
            return new List<Teacher>();
        }
        
        public static async Task<Teacher> GetTeacherAsync(this ApiService service, HttpClient httpClient, string baseUrl, long id)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/teachers/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Teacher>();
            }
            
            return null;
        }
        
        public static async Task<List<Teacher>> GetTeachersByDepartmentAsync(this ApiService service, HttpClient httpClient, string baseUrl, long departmentId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/teachers/department/{departmentId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Teacher>>();
            }
            
            return new List<Teacher>();
        }
        
        public static async Task<List<Teacher>> SearchTeachersAsync(this ApiService service, HttpClient httpClient, string baseUrl, string query)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/teachers/search?query={Uri.EscapeDataString(query)}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Teacher>>();
            }
            
            return new List<Teacher>();
        }
        
        // Subject methods
        public static async Task<List<Subject>> GetSubjectsAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/subjects");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Subject>>();
            }
            
            return new List<Subject>();
        }
        
        public static async Task<Subject> GetSubjectAsync(this ApiService service, HttpClient httpClient, string baseUrl, long id)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/subjects/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Subject>();
            }
            
            return null;
        }
        
        public static async Task<List<Subject>> GetSubjectsByDepartmentAsync(this ApiService service, HttpClient httpClient, string baseUrl, long departmentId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/subjects/department/{departmentId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Subject>>();
            }
            
            return new List<Subject>();
        }
        
        // Schedule methods
        public static async Task<List<Schedule>> GetSchedulesAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/schedule");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Schedule>>();
            }
            
            return new List<Schedule>();
        }
        
        public static async Task<List<Schedule>> GetSchedulesByClassAsync(this ApiService service, HttpClient httpClient, string baseUrl, long classId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/schedule/class/{classId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Schedule>>();
            }
            
            return new List<Schedule>();
        }
        
        public static async Task<List<Schedule>> GetSchedulesByTeacherAsync(this ApiService service, HttpClient httpClient, string baseUrl, long teacherId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/schedule/teacher/{teacherId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Schedule>>();
            }
            
            return new List<Schedule>();
        }
        
        // Notification methods
        public static async Task<List<Notification>> GetNotificationsAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/notifications");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Notification>>();
            }
            
            return new List<Notification>();
        }
        
        public static async Task<List<Notification>> GetNotificationsByUserAsync(this ApiService service, HttpClient httpClient, string baseUrl, long userId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/notifications/user/{userId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Notification>>();
            }
            
            return new List<Notification>();
        }
        
        public static async Task<bool> MarkNotificationAsReadAsync(this ApiService service, HttpClient httpClient, string baseUrl, long id)
        {
            var response = await httpClient.PostAsync($"{baseUrl}/notifications/mark-as-read/{id}", null);
            return response.IsSuccessStatusCode;
        }
        
        public static async Task UnregisterFcmTokenAsync(this ApiService service, HttpClient httpClient, string baseUrl, string deviceId)
        {
            var request = new { DeviceId = deviceId };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"{baseUrl}/notifications/unregister-token", content);
        }
        
        // Department methods
        public static async Task<List<Department>> GetDepartmentsAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/departments");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Department>>();
            }
            
            return new List<Department>();
        }
        
        public static async Task<Department> GetDepartmentAsync(this ApiService service, HttpClient httpClient, string baseUrl, long id)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/departments/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Department>();
            }
            
            return null;
        }
        
        public static async Task<object> GetDepartmentStatisticsAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/departments/statistics");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<object>();
            }
            
            return null;
        }
        
        // Extracurricular methods
        public static async Task<List<Extracurricular>> GetExtracurricularsAsync(this ApiService service, HttpClient httpClient, string baseUrl)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/extracurricular");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Extracurricular>>();
            }
            
            return new List<Extracurricular>();
        }
        
        public static async Task<List<Student>> GetStudentsByExtracurricularAsync(this ApiService service, HttpClient httpClient, string baseUrl, long extracurricularId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/extracurricular/{extracurricularId}/students");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Student>>();
            }
            
            return new List<Student>();
        }
        
        public static async Task<List<Extracurricular>> GetExtracurricularsByStudentAsync(this ApiService service, HttpClient httpClient, string baseUrl, long studentId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/extracurricular/student/{studentId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Extracurricular>>();
            }
            
            return new List<Extracurricular>();
        }
    }
} 