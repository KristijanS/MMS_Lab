using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS_Lab_1
{
    public interface IUndoRedo
    {
        void Undo();
        void Redo();
    }
}
