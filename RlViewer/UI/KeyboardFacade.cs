﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.UI
{
    class KeyboardFacade
    {
        public KeyboardFacade(Action undo, Action openFile, Action saveFile, Action fileInfo, Action log)
        {
            Undo = undo;
            OpenFile = openFile;
            SaveFile = saveFile;
            FileInfo = fileInfo;
            Log = log;
        }

        private Action Undo;
        private Action OpenFile;
        private Action SaveFile;
        private Action FileInfo;
        private Action Log;


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
                    case Keys.L:
                        Log();
                        break;
                    default:
                        break;
                }
            }

        }



    }
}
