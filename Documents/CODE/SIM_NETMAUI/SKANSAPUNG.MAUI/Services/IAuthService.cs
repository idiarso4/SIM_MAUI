using SKANSAPUNG.MAUI.Models;

namespace SKANSAPUNG.MAUI.Services
{
    public interface IAuthService
    {
        Task<string> GetTokenAsync();
        Task<User> GetUserAsync();
        Task<bool> IsAuthenticatedAsync();
        Task SaveTokenAsync(string token);
        Task SaveUserAsync(User user);
        Task ClearAuthAsync();
    }
}