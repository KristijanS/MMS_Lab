using System.Drawing;
using System.Windows.Forms;

namespace MMS_Lab_1.Views
{
    public interface IImageView : IView
    {
        void SetImage(Bitmap bmp);
        Bitmap GetImage();
    }
}
