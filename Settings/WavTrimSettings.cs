using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaLooper.Settings
{
    public class WavTrimSettings : WavSettings
    {
        public double Start { get; set; }
        public double End { get; set; }

        public WavTrimSettings(double start, double end) : base()
        {
            Start = start;
            End = end;
        }
    }
}
