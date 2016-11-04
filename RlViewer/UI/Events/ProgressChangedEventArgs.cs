using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class ProgressChangedEventArgs : EventArgs
    {
        public ProgressChangedEventArgs(int progress)
        {
            _progress = progress;
        }

        private int _progress;

        public int Progress
        {
            get 
            { 
                return _progress; 
            }
        }
    }
}
