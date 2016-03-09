using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using RlViewer.Behaviors.TileCreator;

namespace RlViewer.Behaviors
{
    public class DragController
    {

        private bool _isMouseDown;

        private Point _previousMouseLocation;

        private Point _delta;

        public Point Delta
        {
          get
          {
              return _delta;
          }
        }

        public void StartTracing(Point location, bool canDrag)
        {
            _isMouseDown = canDrag;
            _previousMouseLocation = location;
        }

        public void StopTracing()
        {
            _isMouseDown = false;
        }

        public bool Trace(Point location)
        {
            if (_isMouseDown)
            {
                _delta = new Point(location.X - _previousMouseLocation.X, location.Y - _previousMouseLocation.Y);
                _previousMouseLocation = location;
                //Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("{0}-{1}", _delta.X.ToString(), _delta.Y.ToString()));
            }
            return _isMouseDown;
        }

    }
}
