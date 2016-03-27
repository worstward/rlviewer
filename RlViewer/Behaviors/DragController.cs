﻿using System;
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
        public DragController(Scaling.Scaler scaler)
        {
            _scaler = scaler;
        }


        private Scaling.Scaler _scaler;
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
                _delta = new Point((int)((location.X - _previousMouseLocation.X) / Math.Sqrt(_scaler.ScaleFactor)),
                    (int)((location.Y - _previousMouseLocation.Y) / Math.Sqrt(_scaler.ScaleFactor)));
                _previousMouseLocation = location;
            }
            return _isMouseDown;
        }

    }
}
