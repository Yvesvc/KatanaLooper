using NAudio.Wave;
using System;
using System.Threading;

namespace KatanaLooper.Classes
{
    internal class WavTrimmer
    {
        internal static void Trim(string untrimmedRecordingFilePath, string trimmedRecordingFilePath, double startOfTrimmedWav, double endOfTrimmedWav)
        {
            WaveFileReader reader = new WaveFileReader(untrimmedRecordingFilePath);
            WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
            WaveFileWriter writer = new WaveFileWriter(trimmedRecordingFilePath, waveFormat);
            int startInBytes = (int)(reader.Length * startOfTrimmedWav);
            int bytesPerSample = reader.WaveFormat.BitsPerSample / 8;
            int startByte = bytesPerSample * (int)((double)startInBytes / (double)bytesPerSample);
            int endInBytes = (int)(reader.Length * endOfTrimmedWav);
            int endByte = bytesPerSample * (int)((double)endInBytes / (double)bytesPerSample);

            byte[] bytes = new byte[bytesPerSample];
            for (int i = 0; i < reader.Length; i += bytesPerSample)
            {
                reader.Read(bytes, 0, bytesPerSample);
                if (i >= startByte && i < endByte)
                {

                    writer.Write(bytes, 0, bytesPerSample);
                }
            }

            reader.Dispose();
            writer.Dispose();
        }
    }
}