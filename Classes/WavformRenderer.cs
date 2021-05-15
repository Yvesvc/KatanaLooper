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
using NAudio.MediaFoundation;

namespace KatanaLooper.Classes
{
    public static class WavformRenderer
    {
        static readonly int peakWidth = 2;

        public static Bitmap Render(WavformSettings wavSettings)
        {
            int[] peaks = GetPeaks(wavSettings);
            return BitmapWithPeaks(peaks, wavSettings);
        }

        private static int[] GetPeaks(WavformSettings wavSettings)
        {
            List<int> amplitudes = GetAmplitudes(wavSettings.UntrimmedRecordingFilePath);
            int numberOfPeaks = wavSettings.WavformWidth / peakWidth;
            double amplitudesPerPeak = (double)amplitudes.Count / (double)numberOfPeaks;

            int[] peaks = new int[numberOfPeaks];
            double startIndex = 0;
            for (int i = 0; i < numberOfPeaks; i++)
            {
                peaks[i] = (int)amplitudes.GetRange(Convert.ToInt32(startIndex), Convert.ToInt32(startIndex + amplitudesPerPeak - Convert.ToInt32(startIndex))).Average(); //eg if amplitudes per peak is 2.4 -> 0-2 ; 2-5 ; 5-7 etc 
                startIndex += amplitudesPerPeak;
            }

            return peaks;
        }

        private static List<int> GetAmplitudes(string filePath)
        {
            WaveFileReader wavFileReader = new WaveFileReader(filePath);
            int bytesPerSample = wavFileReader.WaveFormat.BitsPerSample / 8;
            byte[] sample = new byte[bytesPerSample];
            int[] amplitudes = new int[wavFileReader.SampleCount];
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

            for (int i = 0; i < wavFileReader.SampleCount; i++)
            {
                wavFileReader.Read(sample, 0, bytesPerSample);
                amplitudes[i] = Math.Abs(bytesToInt(sample, 0));
            }

            int firstNonZeroAmplitude = Array.FindIndex(amplitudes, a => a > 0); //First samples of wav file might be of amplitude 0

            wavFileReader.Dispose();

            return new List<int>(amplitudes.Skip(firstNonZeroAmplitude).ToArray());
        }

        private static Bitmap BitmapWithPeaks(int[] peaks, WavformSettings wavSettings)
        {
            double amplitudeToBitmapHeightConversionFactor = (double)peaks.Max() / (double)wavSettings.WavformHeight;

            Bitmap wavformBitmap = new Bitmap(wavSettings.WavformWidth, wavSettings.WavformHeight);
            Color color = Color.FromKnownColor(KnownColor.Black);

            int pixelX = 0;
            foreach (int peak in peaks)
            {
                int pixelmaxY = Convert.ToInt32((double)peak / (double)amplitudeToBitmapHeightConversionFactor);
                for (int pixel = 0; pixel < peakWidth; pixel++)
                {
                    for (int pixelY = 0; pixelY < pixelmaxY / 2; pixelY++)
                    {
                        wavformBitmap.SetPixel(pixelX, (wavSettings.WavformHeight / 2) + pixelY, color);//center peak around..
                        wavformBitmap.SetPixel(pixelX, (wavSettings.WavformHeight / 2) - pixelY, color);//..the middle of the bitmap
                    }
                    pixelX++;
                }
            }
            return wavformBitmap;
        }

        internal static Bitmap IncreaseBrightness(Bitmap wavform)
        {
            Bitmap brightWavform = new Bitmap(wavform.Width, wavform.Height);
            for (int x = 0; x < brightWavform.Width; x++)
            {
                for (int y = 0; y < brightWavform.Height; y++)
                {
                    Color color = wavform.GetPixel(x, y);
                    if (color.A == 0 && color.R == 0 && color.G == 0 && color.B == 0)
                    {
                        brightWavform.SetPixel(x, y, Color.FromArgb(0, 255, 255, 255));
                    }
                    else
                    {
                        brightWavform.SetPixel(x, y, Color.FromArgb(255, 127, 127, 127));
                    }
                }
            }
            return brightWavform;
        }
    }

}
