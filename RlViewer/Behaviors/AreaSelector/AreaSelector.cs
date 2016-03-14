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

        public void ResizeArea(Point mouseLocation)
        {
            if (_canResize)
            {
                int width = mouseLocation.X + _pointOfView.X - _initialLocation.X;
                int height = mouseLocation.Y + _pointOfView.Y - _initialLocation.Y;
                Point _location = _initialLocation;

                if (height < 0 && width > 0)
                {
                    _location = new Point(_initialLocation.X, _initialLocation.Y + height);
                    height = -height;
                }
                else if (height > 0 && width < 0)
                {
                    _location = new Point(_initialLocation.X + width, _initialLocation.Y);
                    width = -width;
                }
                else if (width < 0 && height < 0)
                {
                    _location = new Point(_initialLocation.X + width, _initialLocation.Y + height);
                    width = -width;
                    height = -height;
                }

                Area.Location = _location;
                Area.Width = width;
                Area.Height = height;
            }
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
