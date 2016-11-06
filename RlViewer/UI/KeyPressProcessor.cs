using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.UI
{
    class KeyPressProcessor
    {
        public KeyPressProcessor(Action undo, Action openFile, Action saveFile,
            Action fileInfo, Action log, Action report, Action aggregateFiles, 
            Action embedNavigation, Action showCache)
        {
            Undo = undo;
            OpenFile = openFile;
            SaveFile = saveFile;
            FileInfo = fileInfo;
            Log = log;
            Report = report;
            EmbedNavigation = embedNavigation;
            AggregateFiles = aggregateFiles;
            ShowCache = showCache;
        }

        private Action Undo;
        private Action OpenFile;
        private Action SaveFile;
        private Action FileInfo;
        private Action Log;
        private Action Report;
        private Action AggregateFiles;
        private Action EmbedNavigation;
        private Action ShowCache;

        public void ProcessKeyPress(System.Windows.Forms.KeyEventArgs kEvent)
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
                    case Keys.R:
                        Report();
                        break;
                    case Keys.A:
                        AggregateFiles();
                        break;
                    case Keys.E:
                        EmbedNavigation();
                        break;
                    case Keys.C:
                        ShowCache();
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
