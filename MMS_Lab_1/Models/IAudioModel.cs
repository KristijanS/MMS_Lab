using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab_1.Models
{
    public interface IAudioModel
    {
        void Play();
        void Stop();
        void ApplyFilter(int[] channelValues);
        void Load(String path);
        int GetNumberOfChannels();
    }
}
