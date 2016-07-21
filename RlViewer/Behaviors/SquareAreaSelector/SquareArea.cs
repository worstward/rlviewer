using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.SquareAreaSelector
{
    public class SquareArea
    {
        public SquareArea(System.Drawing.Point location, int borderSize)
        {
            _location = location;
            _borderSize = borderSize;
        }

        private int _borderSize;

        public int BorderSize
        {
            get { return _borderSize; }
        }

        private System.Drawing.Point _location;

        public System.Drawing.Point Location
        {
            get { return _location; }
        }

    }
}
