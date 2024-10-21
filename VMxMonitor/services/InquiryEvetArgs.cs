using VMxMonitor.models;

namespace VM_app_221_WAS.services
{
	public class InquiryEventArgs
    {
        /* =====[ properties ]===== */

        public InquiryModel Inquiry
        {
            get
            {
                return mInquiry;
            }
        }
        private InquiryModel mInquiry;

        /* =====[ constructors ]===== */

        public InquiryEventArgs(InquiryModel inquiry)
        {
            // initialize properties.
            mInquiry = inquiry;
        }
    }
}
