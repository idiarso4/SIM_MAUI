using SKANSAPUNG.API.Models;

namespace SKANSAPUNG.API.Services
{
    public interface IFcmTokenService
    {
        Task RegisterTokenAsync(string token, string deviceId, string platform);
        Task DeactivateTokenAsync(string deviceId);
    }
} 