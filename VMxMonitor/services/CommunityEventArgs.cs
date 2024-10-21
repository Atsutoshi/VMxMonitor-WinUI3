using VMxMonitor.models;

namespace VM_app_221_WAS.services
{
	public class CommunityEventArgs
    {
        /* =====[ properties ]===== */

        public CommunityModel community
        {
            get
            {
                return propCommunity;
            }
        }
        private CommunityModel propCommunity;

        /* =====[ constructors ]===== */

        public CommunityEventArgs(CommunityModel argCommunity)
        {
            // initialize properties.
            propCommunity = argCommunity;
        }
    }
}
