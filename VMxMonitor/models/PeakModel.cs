using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMxMonitor.models
{
    public class PeakModel
    {
        /* =====[ constants ]===== */

        public const int STATE_SUCCESS = 0;
        public const int STATE_WARNING = 1;
        public const int STATE_ERROR = 2;
        //
        private const byte UNIT_TYPE_R = 1;
        private const byte UNIT_TYPE_A = 2;
        private const double THRESHOLD_RRAA = 2000.0;
        private const double CONVERSION_RRAA = 300.0;
        private const double THRESHOLD_RA = 400.0;
        private const int PREPARING_BEATS = 10;
        private const int PREPARING_ERRORS = 2;
        private const double VALIDATION_LENGTH = 30000.0;

        /* =====[ properties ]===== */

        public double RR
        {
            get
            {
                return rr;
            }
        }
        public double AA
        {
            get
            {
                return aa;
            }
        }
        public double RA
        {
            get
            {
                return ra;
            }
        }
        public double RT
        {
            get
            {
                return rt;
            }
        }
        public double AT
        {
            get
            {
                return at;
            }
        }
        //
        private double rr;
        private double aa;
        private double ra;
        private double rt;
        private double at;

        /* =====[ static methods ]===== */

        public static List<PeakModel> buildList(List<Unit> units)
        {
            //
            List<PeakModel> peaks = new ();

            // sort units.
            units.Sort((a, b) =>
            {
                if (a.time < b.time)
                {
                    return -1;
                }
                else if (a.time > b.time)
                {
                    return 1;
                }
                return 0;
            });
            //
            List<Unit> tmp = new ();
            double last = 0;
            foreach (Unit unit in units)
            {
                if (last == 0 || Math.Abs(unit.time - last) > 1)
                {
                    tmp.Add(unit);
                    last = unit.time;
                }
            }

            // build list.
            PeakModel peak = null;
            foreach (Unit unit in tmp)
            {
                if (unit.type == UNIT_TYPE_R)
                {
                    if (peak == null)
                    {
                        peak = new PeakModel();
                        peak.rt = unit.time;
                    }
                    else
                    {
                        peaks.Add(peak);
                        //
                        double lastT = 0;
                        if (peak.rt != 0)
                        {
                            lastT = peak.rt;
                        }
                        else if (peak.at != 0)
                        {
                            lastT = peak.at - CONVERSION_RRAA;
                        }
                        int n = (int)Math.Floor((unit.time - lastT) / THRESHOLD_RRAA);
                        while (n > 0)
                        {
                            peaks.Add(new PeakModel());
                            --n;
                        }
                        peak = new PeakModel();
                        peak.rt = unit.time;
                    }
                }
                else if (unit.type == UNIT_TYPE_A)
                {
                    if (peak == null)
                    {
                        peak = new PeakModel();
                        peak.at = unit.time;
                    }
                    else if (peak.rt != 0 && unit.time - peak.rt < THRESHOLD_RA)
                    {
                        peak.at = unit.time;
                    }
                    else
                    {
                        peaks.Add(peak);
                        //
                        double lastT = 0;
                        if (peak.at != 0)
                        {
                            lastT = peak.at;
                        }
                        else if (peak.rt != 0)
                        {
                            lastT = peak.rt + CONVERSION_RRAA;
                        }
                        int n = (int)Math.Floor((unit.time - lastT) / THRESHOLD_RRAA);
                        while (n > 0)
                        {
                            peaks.Add(new PeakModel());
                            --n;
                        }
                        peak = new PeakModel();
                        peak.at = unit.time;
                    }
                    peaks.Add(peak);
                    peak = null;
                }
            }
            if (peak != null)
            {
                peaks.Add(peak);
            }
            //
            PeakModel lastP = null;
            foreach (PeakModel currP in peaks)
            {
                if (lastP != null)
                {
                    if (currP.rt != 0 && lastP.rt != 0)
                    {
                        currP.rr = currP.rt - lastP.rt;
                    }
                    if (currP.at != 0 && lastP.at != 0)
                    {
                        currP.aa = currP.at - lastP.at;
                    }
                    if(currP.rt != 0 && currP.at != 0)
                    {
                        currP.ra = currP.at - currP.rt;
                    }
                }
                lastP = currP;
            }
            if (peaks.Count() > 0)
            {
                peaks.RemoveAt(0);
            }

            //
            return peaks;
        }

        public static bool prepare(List<PeakModel> peaks)
        {
            // collect.
            List<double> rraas = new();
            foreach (PeakModel peak in peaks)
            {
                if (peak.rr != 0)
                {
                    rraas.Add(peak.rr);
                }
                else if (peak.aa != 0)
                {
                    rraas.Add(peak.aa);
                }
                else
                {
                    rraas.Add(0);
                }
            }
            while (rraas.Count() > PREPARING_BEATS)
            {
                rraas.RemoveAt(0);
            }
            if (rraas.Count() >= PREPARING_BEATS)
            {
                // get median.
                List<double> data = new();
                foreach (double rraa in rraas)
                {
                    if (rraa != 0)
                    {
                        data.Add(rraa);
                    }
                }
                double med = median(data);

                // validate.
                int n = 0;
                foreach (double rraa in rraas)
                {
                    if (rraa < med * (3.0 / 4.0) || rraa > med * (7.0 / 4.0))
                    {
                        ++n;
                    }
                }
                if (n < PREPARING_ERRORS)
                {
                    return true;
                }
            }

            //
            return false;
        }

        public static int validate(List<PeakModel> peaks)
        {
            // collect.
            double start = 0;
            double end = 0;
            foreach (PeakModel peak in peaks)
            {
                if(start == 0)
                {
                    if (peak.rt != 0)
                    {
                        start = peak.rt;
                    }
                    else if(peak.at != 0)
                    {
                        start = peak.at;
                    }
                }
                if (peak.rt != 0)
                {
                    end = peak.rt;
                }
                else if (peak.at != 0)
                {
                    end = peak.at;
                }
            }
            if(start != 0 && end - start >= VALIDATION_LENGTH)
            {
                // get median.
                List<double> data = new ();
                foreach (PeakModel peak in peaks)
                {
                    if ((peak.rt != 0 && peak.rt - start >= VALIDATION_LENGTH) || (peak.at !=0 && peak.at - start >= VALIDATION_LENGTH))
                    {
                        break;
                    }
                    if(peak.rr != 0)
                    {
                        data.Add(peak.rr);
                    }
                    else if (peak.aa != 0)
                    {
                        data.Add(peak.aa);
                    }
                }
                double med = median(data);

                // validate.
                int[] indexes = new int[] { -1, -1, -1 };
                bool invalid = false;
                for(int i = 0; i < peaks.Count(); ++i)
                {
                    double rraa = (peaks[i].rr != 0) ? peaks[i].rr : peaks[i].aa;
                    if(rraa == 0 ||  rraa < med * (3.0 / 4.0) || rraa > med * (7.0 / 4.0))
                    {
                        indexes[0] = indexes[1];
                        indexes[1] = indexes[2];
                        indexes[2] = i;
                        if(indexes[0] >= 0 && indexes[2] - indexes[0] <= 10)
                        {
                            return STATE_ERROR;
                        }
                        else if(indexes[1] >= 0 && indexes[2] - indexes[1] <= 10)
                        {
                            invalid = true;
                        }
                    }
                }
                if (invalid)
                {
                    return STATE_WARNING;
                }
            }

            //
            return STATE_SUCCESS;
        }

        private static double median(List<double> data)
        {
            if(data.Count() == 0)
            {
                return 0;
            }
            data.Sort((a, b) =>
            {
                if (a < b)
                {
                    return -1;
                }
                else if (a > b)
                {
                    return 1;
                }
                return 0;
            });
            if (data.Count() % 2 == 0)
            {
                return (data[data.Count() / 2 - 1] + data[data.Count() / 2]) / 2.0;
            }
            else
            {
                return data[data.Count() / 2];
            }
        }

        /* =====[ constructors ]===== */

        public PeakModel()
        {
            // initialize properties.
            rr = 0;
            aa = 0;
            ra = 0;
            rt = 0;
            at = 0;
        }

        /* =====[ Unit ]===== */

        public struct Unit
        {
            /* -----( properties )----- */

            public byte type;
            public double time;

            /* -----( constructors )----- */

            public Unit(byte argType, double argTime)
            {
                type = argType;
                time = argTime;
            }
        }

    }
}
