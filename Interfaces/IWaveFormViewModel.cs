using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KatanaLooper
{
    interface IWaveformViewModel
    {
        bool Recording { get; set; }
        bool Playing { get; set; }
        bool StreamEnded { get; set; }
        string RecordWavCommandText { get; set; }
        string RecordWavCommandImage { get; set; }

        ICommand RecordWavCommand { get; set; }
        ICommand RenderWaveformCommand { get; set; }
        ICommand UpdateStartAndEndOfWavCommand { get; set; }
        ICommand PlayWavCommand { get; set; }
        ICommand SaveWavCommand { get; set; }

        int WaveformWidth { get; set; }
        int WaveformHeight { get; set; }
        Bitmap Waveform { get; set; }
        Bitmap GreyedOutWaveform { get; set; }
        double LengthSongInSec { get; set; }

        event Action WavLoaded;
    }
}
