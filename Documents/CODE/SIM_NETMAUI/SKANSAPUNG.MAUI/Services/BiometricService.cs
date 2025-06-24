using Plugin.Fingerprint.Abstractions;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.Services
{
    public class BiometricService : IBiometricService
    {
        private readonly IFingerprint _fingerprint;

        public BiometricService(IFingerprint fingerprint)
        {
            _fingerprint = fingerprint;
        }

        public async Task<bool> IsAvailableAsync()
        {
            return await _fingerprint.IsAvailableAsync(true);
        }

        public async Task<bool> AuthenticateAsync(string reason)
        {
            var request = new AuthenticationRequestConfiguration("Login", reason);
            var result = await _fingerprint.AuthenticateAsync(request);
            return result.Authenticated;
        }
    }
}
