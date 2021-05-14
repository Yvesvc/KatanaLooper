using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaLooper.Settings
{
    public class WaveformSettings : WavSettings
    {
        public int WaveformWidth { get; }
        public int WaveformHeight { get; }

        public WaveformSettings(int waveFormWidth, int waveFormHeight) : base()
        {
            WaveformWidth = waveFormWidth;
            WaveformHeight = waveFormHeight;
        }
    }

}
