using KatanaLooper.Settings;
using NAudio.Wave;
using System.IO;

namespace KatanaLooper.Classes
{
    public class Recorder : AsioOut
    {
        readonly WaveFileWriter wavFileWriter;
        readonly WavSettings wavSettings;
        public Recorder(WavSettings wavSettings, int channelOffset, int channelCount, int sampleRate) : base("KATANA")
        {
            this.wavSettings = wavSettings;
            InputChannelOffset = channelOffset;
            InitRecordAndPlayback(null, channelCount, sampleRate);
            AudioAvailable += OnAudioAvailable;
            wavFileWriter = new WaveFileWriter(this.wavSettings.UntrimmedRecordingFilePath, WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
        }

        private void OnAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
        {
            float[] samples = new float[1024];
            e.GetAsInterleavedSamples(samples);
            wavFileWriter.WriteSamples(samples, 0, e.SamplesPerBuffer);
            
            if (wavFileWriter.Position > wavFileWriter.WaveFormat.AverageBytesPerSecond * 30)
            {
                Stop();// stop recording after 30sec to prevent wav file to big (max size is 4GB)
            }
        }

        public void StartRecording()
        {
            Play();
        }

        public void StopRecording()
        {
            Stop();
            wavFileWriter.Dispose();
            File.Copy(wavSettings.UntrimmedRecordingFilePath, wavSettings.TrimmedRecordingFilePath, true);
        }

    }
}
