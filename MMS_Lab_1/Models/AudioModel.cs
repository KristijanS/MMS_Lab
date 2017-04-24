using System;
using System.IO;
using System.Media;

namespace MMS_Lab_1.Models
{
    public class AudioModel : IAudioModel
    {
        private SoundPlayer player;

        public AudioModel()
        {
            player = new SoundPlayer();
        }

        public void ApplyFilter(int[] channelValues)
        {
            byte[] bytes = File.ReadAllBytes(player.SoundLocation);
            int startOffset = DataOffset(bytes);
            int dataLength = bytes.Length - startOffset;
            byte[] data = new byte[dataLength];

            int channel = 0;
            int segmentCount = 0;

            for (int i = 0; i < dataLength; i++)
            {
                if (segmentCount >= 2)
                {
                    channel++;
                    segmentCount = 0;
                }

                if (channel >= channelValues.Length)
                {
                    channel = 0;
                }

                data[i] = (byte)(Math.Abs(bytes[startOffset + i] - channelValues[channel]) % 255);
                segmentCount++;
            }

            FileStream fstream = new FileStream(player.SoundLocation + "_modded.wav", FileMode.Create);
            BinaryWriter bwr = new BinaryWriter(fstream);

            //header
            for (int i = 0; i < startOffset; i++)
            {
                bwr.Write(bytes[i]);
            }
            //data
            for (int i = 0; i < data.Length; i++)
            {
                bwr.Write(data[i]);
            }

            fstream.Close();
            bwr.Close();

            //load and play the modded wav file
            player.Stop();
            player.SoundLocation += "_modded.wav";
            player.Load();
            player.Play();
        }

        public int GetNumberOfChannels()
        {
            int channels = 0;
            try
            {
                BinaryReader reader = new BinaryReader(new FileStream(player.SoundLocation, FileMode.Open));

                int chunkID = reader.ReadInt32();
                int fileSize = reader.ReadInt32();
                int riffType = reader.ReadInt32();
                int fmtID = reader.ReadInt32();
                int fmtSize = reader.ReadInt32();
                int fmtCode = reader.ReadInt16();
                channels = reader.ReadInt16();

                reader.Close();

                return channels;
            }
            catch
            {
                return 0;
            }

        }

        public void Load(string path)
        {
            player.SoundLocation = path;
            player.Load();
        }

        public void Play()
        {
            player.Play();
        }

        public void Stop()
        {
            player.Stop();
        }

        private int DataOffset(byte[] data)
        {
            int offset = 0;
            try
            {
                while (true)
                {
                    if (data[offset] == 'd' && data[offset + 1] == 'a' && data[offset + 2] == 't' && data[offset + 3] == 'a')
                        break;
                    offset++;
                }
                return offset + 2 * 4;
            }
            catch
            {
                return 0;
            }
        }
    }
}
