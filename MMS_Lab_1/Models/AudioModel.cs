using System;
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

        public void ApplyFilter()
        {
            throw new NotImplementedException();
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

        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            player.Stop();
        }
    }
}
