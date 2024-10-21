using VMxMonitor.models;

namespace VM_app_221_WAS.services
{
	public class UserEventArgs
    {
        /* =====[ properties ]===== */

        public UserModel User
        {
            get
            {
                return mUser;
            }
        }
        private UserModel mUser;

        /* =====[ constructors ]===== */

        public UserEventArgs(UserModel user)
        {
            // initialize properties.
            mUser = user;
        }
    }
}
