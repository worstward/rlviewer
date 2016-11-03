using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class ProgressControlsVisibilityEventArgs : EventArgs
    {
        public ProgressControlsVisibilityEventArgs(bool isVisible)
        {
            _isVisible = isVisible;
        }

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
        }
    }
}
