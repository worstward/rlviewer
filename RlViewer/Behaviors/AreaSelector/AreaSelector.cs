using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.AreaSelector
{
    public class AreaSelector
    {
        private Point _initialLocation;
        private Point _pointOfView;
        private bool _canResize;

        /// <summary>
        /// Resizes selected area
        /// </summary>
        /// <param name="mouseLocation">Current cursor position</param>
        /// <param name="pointOfView">Current canvas top left point</param>
        /// <returns>Value, that determines if resizing was successful</returns>
        public bool ResizeArea(Point mouseLocation, Point pointOfView)
        {
            if (_canResize)
            {

                int width = mouseLocation.X + pointOfView.X - _initialLocation.X;
                int height = mouseLocation.Y + pointOfView.Y - _initialLocation.Y;


                if (height > 0 && width > 0)
                {
                    Area.Location = _initialLocation;
                }
                else if (height < 0 && width > 0)
                {
                    Area.Location = new Point(_initialLocation.X, _initialLocation.Y + height);
                    height = -height;
                }
                else if (height > 0 && width < 0)
                {
                    Area.Location = new Point(_initialLocation.X + width, _initialLocation.Y);
                    width = -width;
                }
                else if (width < 0 && height < 0)
                {
                    Area.Location = new Point(_initialLocation.X + width, _initialLocation.Y + height);
                    width = -width;
                    height = -height;
                }


                Area.Width = width;
                Area.Height = height;
            }
            return _canResize;
            // _bytesPerAreaLine = _area.Width * _loc.Header.BytesPerSample;
        }

        public void ResetArea()
        {
            Area = null;
        }


        private SelectedArea _area;
        public SelectedArea Area
        {
            get
            {
                _area = _area ?? new SelectedArea();
                return _area;
            }
            private set
            {
                _area = value;
            }
        }

        public void StartArea(Point location, Point pointOfView)
        {
            _canResize = true;
            _pointOfView = pointOfView;
            _initialLocation = new Point(location.X + pointOfView.X, location.Y + pointOfView.Y);
        }

        public void StopResizing()
        {
            _canResize = false;
        }

    }
}
