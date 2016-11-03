
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.UI.Events
{
    public class AlignPossibilityEventArgs : EventArgs
    {
        public AlignPossibilityEventArgs(bool isPossible)
        {
            _isPossible = isPossible;
        }

        private bool _isPossible;

        public bool IsPossible
        {
            get
            {
                return _isPossible;
            }
        }
    }
}
