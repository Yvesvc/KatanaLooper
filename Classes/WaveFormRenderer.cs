using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KatanaLooper.Settings;
using System.Linq.Expressions;
using System.IO;

namespace KatanaLooper.Classes
{
    public static class WaveformRenderer
    {
        static readonly int peakWidth = 2;
        private delegate int ToInt(byte[] value, int startIndex);

        public static Bitmap Render(WaveformSettings wavSettings)
        {
            WaveFileReader reader = new WaveFileReader(wavSettings.UntrimmedRecordingFilePath);
            int[] peaks = GetPeaks(reader, wavSettings);
            reader.Dispose();
            return DrawPeaksOnBitMap(peaks, wavSettings);
        }

        private static int[] GetPeaks(WaveFileReader reader, WaveformSettings wavSettings)
        {
            int numberOfPeaks =  wavSettings.WaveformWidth / peakWidth;
            List<int> amplitudes = GetAmplitudes(reader);
            double samplesPerPeak = (double)amplitudes.Count / (double)numberOfPeaks;

            int[] peaks = new int[numberOfPeaks];
            double startIndex = 0;
            for (int i = 0; i < numberOfPeaks; i++)
            {
                try
                {
                    peaks[i] = (int)amplitudes.GetRange(Convert.ToInt32(startIndex), Convert.ToInt32(startIndex + samplesPerPeak - Convert.ToInt32(startIndex))).Average();
                    startIndex += samplesPerPeak;
                }
                catch
                {

                }
                
            }
            return peaks;
        }

        private static List<int> GetAmplitudes(WaveFileReader reader)
        {
            int bytesPerSample = reader.WaveFormat.BitsPerSample / 8;
            byte[] sample = new byte[bytesPerSample];
            int[] amplitudes = new int[reader.SampleCount];
            Func<byte[], int, int> bytesToInt;
            switch (bytesPerSample)
            {
                case 2:
                    bytesToInt = delegate (byte[] value, int startIndex) { return BitConverter.ToInt16(value, startIndex); };
                    break;
                case 4:
                    bytesToInt = delegate (byte[] value, int startIndex) { return BitConverter.ToInt32(value, startIndex); };
                    break;
                case 8:
                    bytesToInt = delegate (byte[] value, int startIndex) { return (int)BitConverter.ToInt64(value, startIndex); };
                    break;
                default:
                    throw new Exception("Waveformat is not in format of 2, 4, or 8 bytes per sample");
            }

            for (int index = 0; index < reader.SampleCount; index++)
            {
                reader.Read(sample, 0, bytesPerSample);
                amplitudes[index] = Math.Abs(bytesToInt(sample, 0));
            }

            int trimmedIndex = Array.FindIndex(amplitudes, a => a > 0);

            return new List<int>(amplitudes.Skip(trimmedIndex).ToArray());
        }

        private static Bitmap DrawPeaksOnBitMap(int[] peaks, WaveformSettings wavSettings)
        {
            double amplitudeToImageHeightConversionFactor = (double)peaks.Max() / (double)wavSettings.WaveformHeight;

            Bitmap waveformBitmap = new Bitmap(wavSettings.WaveformWidth, wavSettings.WaveformHeight);
            Color color = Color.FromKnownColor(KnownColor.Black);

            int pixelX = 0;
            foreach (int peak in peaks)
            {
                int pixelmaxY = Convert.ToInt32((double)peak / (double)amplitudeToImageHeightConversionFactor);
                for (int pixel = 0; pixel < peakWidth; pixel++)
                {
                    for (int pixelY = 0; pixelY < pixelmaxY / 2; pixelY++)
                    {
                        try
                        {
                            waveformBitmap.SetPixel(pixelX, (wavSettings.WaveformHeight / 2) + pixelY, color);
                            waveformBitmap.SetPixel(pixelX, (wavSettings.WaveformHeight / 2) - pixelY, color);
                        }
                        

                        catch 
                        {

                        }
                    }
                    pixelX++;
                }
            }
            return waveformBitmap;
        }

        internal static Bitmap IncreaseBrightness(Bitmap waveform)
        {
            Bitmap brightWaveform = new Bitmap(waveform.Width, waveform.Height);
            for (int x = 0; x < brightWaveform.Width; x++)
            {
                for (int y = 0; y < brightWaveform.Height; y++)
                {
                    Color color = waveform.GetPixel(x, y);
                    if (color.A == 0 && color.R == 0 && color.G == 0 && color.B == 0)
                    {
                        brightWaveform.SetPixel(x, y, Color.FromArgb(0, 255, 255, 255));
                    }
                    else
                    {
                        brightWaveform.SetPixel(x, y, Color.FromArgb(255, 127, 127, 127));
                    }
                }
            }
            return brightWaveform;
        }
    }

}
