using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab_1.Controllers
{
    public interface IAudioController : IController
    {
        void Play();
        void Stop();
        void Load(String path);
        void ApplyFilter(int[] channelValues);
        int GetNumberOfChannels();
    }
}
