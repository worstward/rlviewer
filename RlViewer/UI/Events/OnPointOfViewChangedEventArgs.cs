using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class OnPointOfViewChangedEventArgs : EventArgs
    {
        public OnPointOfViewChangedEventArgs(int powAxisValue)
        {
            _powAxisValue = powAxisValue;
        }

        private int _powAxisValue;

        public int AxisValue
        {
            get
            {
                return _powAxisValue;
            }
        }


    }
}
