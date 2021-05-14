using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KatanaLooper
{
    interface IWavformViewModel //interface between View (MainWindow) and ViewModel (MainViewModel)
    {
        bool Recording { get; set; }
        bool Playing { get; set; }
        bool StreamEnded { get; set; }
        string RecordWavCommandText { get; set; }
        string RecordWavCommandImage { get; set; }
        int WavformWidth { get; set; }
        int WavformHeight { get; set; }
        Bitmap Wavform { get; set; }
        Bitmap GreyedOutWavform { get; set; }
        double LengthRecordingInSec { get; set; }

        ICommand RecordWavCommand { get; set; }
        ICommand UpdateStartAndEndOfWavCommand { get; set; }
        ICommand PlayWavCommand { get; set; }
        ICommand SaveWavCommand { get; set; }

        event Action WavLoaded;
    }
}
