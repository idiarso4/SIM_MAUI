namespace SKANSAPUNG.MAUI.Services
{
    public interface IConnectivityService
    {
        bool IsConnected { get; }
        void SimulateOffline(bool isOffline);
    }
} 