using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMS_Lab_1.Views
{
    public interface IView
    {
        void SetParentForm(Form f);
        void SetVisibility(bool value);
    }
}
