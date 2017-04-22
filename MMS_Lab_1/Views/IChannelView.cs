using System.Drawing;

namespace MMS_Lab_1.Views
{
    public interface IChannelView
    {
        void SetChannelX(Bitmap bmp);
        void SetChannelY(Bitmap bmp);
        void SetChannelZ(Bitmap bmp);
        Bitmap GetChannelX();
        Bitmap GetChannelY();
        Bitmap GetChannelZ();
    }
}
