namespace SKANSAPUNG.MAUI.Services
{
    public interface IGeolocationService
    {
        Task<Microsoft.Maui.Devices.Sensors.Location> GetCurrentLocationAsync();
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
        Task<bool> IsWithinRadiusAsync(double targetLatitude, double targetLongitude, double radiusInMeters);
        Task<bool> CheckLocationPermissionAsync();
        Task<bool> RequestLocationPermissionAsync();
    }
} 