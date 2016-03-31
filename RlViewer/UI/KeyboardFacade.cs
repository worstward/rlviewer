using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.UI
{
    class KeyboardFacade
    {
        public KeyboardFacade(Action undo, Action openFile, Action saveFile, Action fileInfo)
        {
            Undo = undo;
            OpenFile = openFile;
            SaveFile = saveFile;
            FileInfo = fileInfo;
        }

        Action Undo;
        Action OpenFile;
        Action SaveFile;
        Action FileInfo;

        public void ProceedKeyPress(System.Windows.Forms.KeyEventArgs kEvent)
        {
            if (kEvent.Control)
            {

                switch (kEvent.KeyCode)
                {
                    case Keys.Z:
                    case Keys.Escape:
                        Undo();
                        break;
                    case Keys.O:
                        OpenFile();
                        break;
                    case Keys.S:
                        SaveFile();
                        break;
                    case Keys.I:
                        FileInfo();
                        break;
                    default:
                        break;
                }
            }

        }



    }
}
