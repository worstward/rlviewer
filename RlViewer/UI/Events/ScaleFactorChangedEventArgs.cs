using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class ScaleFactorChangedEventArgs : EventArgs
    {
        public ScaleFactorChangedEventArgs(float scaleFactor)
        {
            _scaleFactor = scaleFactor;
        }

        private float _scaleFactor;

        public float ScaleFactor
        {
            get { return _scaleFactor; }
        }

    }
}
