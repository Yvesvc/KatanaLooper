using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaLooper.Settings
{
    public class WavformSettings : WavSettings
    {
        public int WavformWidth { get; }
        public int WavformHeight { get; }

        public WavformSettings(int wavFormWidth, int wavFormHeight) : base()
        {
            WavformWidth = wavFormWidth;
            WavformHeight = wavFormHeight;
        }
    }

}
