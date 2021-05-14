using KatanaLooper.Settings;
using NAudio.Wave;
using System.IO;

namespace KatanaLooper.Classes
{
    public class Recorder : AsioOut
    {
        WaveFormat waveFormat;
        WaveFileWriter writer;
        readonly WavSettings _wavSettings;
        public Recorder(WavSettings wavSettings, int channelOffset, int channelCount, int sampleRate) : base("KATANA")
        {
            _wavSettings = wavSettings;
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
            writer = new WaveFileWriter(_wavSettings.UntrimmedRecordingFilePath, waveFormat);

            InputChannelOffset = channelOffset;
            InitRecordAndPlayback(null, channelCount, sampleRate);
            AudioAvailable += OnAsioOutAudioAvailable;
        }

        private void OnAsioOutAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
        {
            float[] samples = new float[1024];
            e.GetAsInterleavedSamples(samples);
            writer.WriteSamples(samples, 0, e.SamplesPerBuffer);
            // stop recording after 30sec to prevent wav file to big (max size is 4GB)
            if (writer.Position > waveFormat.AverageBytesPerSecond * 30)
            {
                Stop();
            }
        }

        public void StartRecording()
        {
            Play();
        }

        public void StopRecording()
        {
            Stop();
            writer.Dispose();
            File.Copy(_wavSettings.UntrimmedRecordingFilePath, _wavSettings.TrimmedRecordingFilePath, true);
        }

    }
}
