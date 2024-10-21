using VMxMonitor.models;

namespace VM_app_221_WAS.services
{
	public class MonitorEventArgs
    {
        /* =====[ properties ]===== */

        public MonitorModel Monitor
        {
            get
            {
                return mMonitor;
            }
        }
        private MonitorModel mMonitor;

        /* =====[ constructors ]===== */

        public MonitorEventArgs(MonitorModel monitor)
        {
            // initialize properties.
            mMonitor = monitor;
        }
    }
}
