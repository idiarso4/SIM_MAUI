using SKANSAPUNG.MAUI.Models;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace SKANSAPUNG.MAUI.Services
{
    public class AuthService : IAuthService
    {
        private const string AuthTokenKey = "AuthToken";
        private const string UserInfoKey = "UserInfo";
        private readonly ISecureStorage _secureStorage;

        public AuthService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _secureStorage.GetAsync(AuthTokenKey);
        }

        public async Task<User> GetUserAsync()
        {
            var userJson = await _secureStorage.GetAsync(UserInfoKey);
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonSerializer.Deserialize<User>(userJson);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task SaveTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                await _secureStorage.RemoveAsync(AuthTokenKey);
            else
                await _secureStorage.SetAsync(AuthTokenKey, token);
        }

        public async Task SaveUserAsync(User user)
        {
            if (user == null)
                await _secureStorage.RemoveAsync(UserInfoKey);
            else
            {
                var userJson = JsonSerializer.Serialize(user);
                await _secureStorage.SetAsync(UserInfoKey, userJson);
            }
        }

        public async Task ClearAuthAsync()
        {
            await _secureStorage.RemoveAsync(AuthTokenKey);
            await _secureStorage.RemoveAsync(UserInfoKey);
            await Task.CompletedTask;
        }
    }
}