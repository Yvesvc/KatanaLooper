using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaLooper.Classes
{
    //https://markheath.net/post/looped-playback-in-net-with-naudio
    class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public event Action StreamEnded;

        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }


        public override long Length
        {
            get { return sourceStream.Length; }
        }


        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                    StreamEnded.Invoke();
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
