using NAudio.Wave;
using System;
using System.Threading;

namespace KatanaLooper.Classes
{
    static internal class WavTrimmer
    {
        internal static void Trim(string untrimmedRecordingFilePath, string trimmedRecordingFilePath, double startOfTrimmedWav, double endOfTrimmedWav)
        {
            WaveFileReader wavFileReader = new WaveFileReader(untrimmedRecordingFilePath);
            WaveFileWriter wavFileWriter = new WaveFileWriter(trimmedRecordingFilePath, WaveFormat.CreateIeeeFloatWaveFormat(wavFileReader.WaveFormat.SampleRate, wavFileReader.WaveFormat.Channels));
            int startByte = (int)(wavFileReader.Length * startOfTrimmedWav);
            int bytesPerSample = wavFileReader.WaveFormat.BitsPerSample / 8;
            int startByteOfSample = bytesPerSample * (int)((double)startByte / (double)bytesPerSample);
            int endByte = (int)(wavFileReader.Length * endOfTrimmedWav);
            int endByteOfSample = bytesPerSample * (int)((double)endByte / (double)bytesPerSample);

            byte[] sample = new byte[bytesPerSample];
            for (int i = 0; i < wavFileReader.Length; i += bytesPerSample)
            {
                wavFileReader.Read(sample, 0, bytesPerSample);
                if (i >= startByteOfSample && i < endByteOfSample)
                {
                    wavFileWriter.Write(sample, 0, bytesPerSample);
                }
            }

            wavFileReader.Dispose();
            wavFileWriter.Dispose();
        }
    }
}