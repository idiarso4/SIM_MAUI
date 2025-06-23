using Microsoft.Maui.Networking;

namespace SKANSAPUNG.MAUI.Services
{
    public class ConnectivityService : IConnectivityService
    {
        private bool _isSimulatingOffline = false;

        public bool IsConnected
        {
            get
            {
#if DEBUG
                if (_isSimulatingOffline)
                {
                    return false;
                }
#endif
                return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            }
        }

        public void SimulateOffline(bool isOffline)
        {
#if DEBUG
            _isSimulatingOffline = isOffline;
#endif
        }
    }
} 