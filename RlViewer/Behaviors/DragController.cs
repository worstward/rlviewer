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

        private bool isMouseDown;

        private Point previousMouseLocation;

        private Point delta;

        public Point Delta
        {
          get
          {
              return delta;
          }
        }

        public void StartTracing(Point location, bool canDrag)
        {
            isMouseDown = canDrag;
            previousMouseLocation = location;
        }

        public void StopTracing()
        {
            isMouseDown = false;
        }

        public bool Trace(Point location)
        {
            if (isMouseDown)
            {
                delta = new Point(location.X - previousMouseLocation.X, location.Y - previousMouseLocation.Y);
                previousMouseLocation = location;
                //Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("{0}-{1}", delta.X.ToString(), delta.Y.ToString()));
            }
            return isMouseDown;
        }

    }
}
