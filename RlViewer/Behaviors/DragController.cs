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

    /// <summary>
    /// Incapsulates panning functions
    /// </summary>
    public class DragController
    {

        /// <summary>
        /// Distance between last and current mouse positions
        /// </summary>
        public Point Delta
        {
            get;
            private set;
        }

        private bool _isMouseDown;
        private Point _previousMouseLocation;

        /// <summary>
        /// Enables <see cref="Trace"/> method
        /// </summary>
        /// <param name="location">Current mouse location</param>
        /// <param name="canDrag">Determines if panning is allowed</param>
        public void StartTracing(Point location, bool canDrag)
        {
            _isMouseDown = canDrag;
            _previousMouseLocation = location;
        }


        /// <summary>
        /// Disables <see cref="Trace"/> method
        /// </summary>
        public void StopTracing()
        {
            _isMouseDown = false;
        }


        /// <summary>
        /// Calculates distance between current and previous mouse positions and writes it to <see cref="Delta"/> property
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool Trace(Point location)
        {
            if (_isMouseDown)
            {
                Delta = new Point(location.X - _previousMouseLocation.X, location.Y - _previousMouseLocation.Y);
                _previousMouseLocation = location;
            }
            return _isMouseDown;
        }

    }
}
