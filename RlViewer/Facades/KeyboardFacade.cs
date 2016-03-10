using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Facades
{
    class KeyboardFacade
    {
        public KeyboardFacade(Action undo, Action openFile)
        {
            Undo = undo;
            OpenFile = openFile;
        }

        Action Undo;
        Action OpenFile;

        public void ProceedKeyPress(System.Windows.Forms.KeyEventArgs kEvent)
        {
            if (kEvent.Control && kEvent.KeyCode == System.Windows.Forms.Keys.Z 
                || kEvent.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                Undo();
            }
            else if (kEvent.Control && kEvent.KeyCode == System.Windows.Forms.Keys.O)
            {
                OpenFile();
            }

        }



    }
}
