using System.Drawing;
using System.Windows.Forms;

namespace MMS_Lab_1.Views
{
    public interface IView
    {
        void SetParentForm(Form f);
        void SetImage(Bitmap bmp);
        Bitmap GetImage();
        void SetVisibility(bool value);
    }
}
