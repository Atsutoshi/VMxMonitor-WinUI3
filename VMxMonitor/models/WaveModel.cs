using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMxMonitor.models
{
    public class WaveModel
    {
        /* =====[ properties ]===== */

        public short ECG
        {
            get
            {
                return ecg;
            }
        }
        public short PPG
        {
            get
            {
                return ppg;
            }
        }
        public short APG
        {
            get
            {
                return apg;
            }
        }
        //
        private short ecg;
        private short ppg;
        private short apg;

        /* =====[ constructors ]===== */

        public WaveModel(short argECG, short argPPG, short argAPG)
        {
            // initialize properties.
            ecg = argECG;
            ppg = argPPG;
            apg = argAPG;
        }
    }
}
