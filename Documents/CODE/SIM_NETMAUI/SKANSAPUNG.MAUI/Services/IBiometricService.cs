namespace SKANSAPUNG.MAUI.Services
{
    public interface IBiometricService
    {
        Task<bool> IsAvailableAsync();
        Task<bool> AuthenticateAsync(string reason);
    }
}
