using MMS_Lab_1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab_1.Controllers
{
    public interface IController
    {
        void AttachView(IView view);
    }
}
