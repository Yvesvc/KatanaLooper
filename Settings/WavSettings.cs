using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaLooper.Settings
{
    public class WavSettings
    {
        public string RecordingDir { get => GeneralSettings.RecordingsDirectory; }
        public string UntrimmedRecordingFilePath { get => RecordingDir + "UntrimmedRecording.wav"; }
        public string TrimmedRecordingFilePath { get => RecordingDir + "TrimmedRecording.wav"; }

        public WavSettings ()
        {
        }
    }
}
