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
        public AreaSelector(Files.LocatorFile file)
        {
            _file = file;
        }

        private Files.LocatorFile _file;
        
        private Point _initialLocation;
        private Point _pointOfView;
        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        /// <summary>
        /// Resizes selected area
        /// </summary>
        /// <param name="mouseLocation">Current cursor position</param>
        /// <param name="pointOfView">Current canvas top left point</param>
        /// <returns>Value, that determines if resizing was successful</returns>
        public bool ResizeArea(Point mouseLocation, Point pointOfView)
        {
            if (_isActive)
            {
                int width = mouseLocation.X + pointOfView.X - _initialLocation.X;
                int height = mouseLocation.Y + pointOfView.Y - _initialLocation.Y;

                if (height > 0)
                {
                    if (width > 0)
                    {
                        Area.Location = _initialLocation;
                    }
                    else
                    {
                        Area.Location = new Point(_initialLocation.X + width, _initialLocation.Y);
                        width = -width;
                    }
                }
                else
                {
                    if (width > 0)
                    {
                        Area.Location = new Point(_initialLocation.X, _initialLocation.Y + height);
                        height = -height;
                    }
                    else
                    {
                        Area.Location = new Point(_initialLocation.X + width, _initialLocation.Y + height);
                        width = -width;
                        height = -height;
                    }
                }

                //we add 1 to avoid need of full pixel envelopment to take it into area
                //so even if we cover part of pixel it will get inside
                width += 1;
                height += 1;

                if (Area.Location.X < 0)
                {
                    Area.Location = new Point(0, Area.Location.Y);
                }
                if (Area.Location.Y < 0)
                {
                    Area.Location = new Point(Area.Location.X, 0);
                }

                if (Area.Location.X + width > _file.Width)
                {
                    width = _file.Width - Area.Location.X;
                }

                if (Area.Location.Y + height > _file.Height)
                {
                    height = _file.Height - Area.Location.Y;
                }


                if (Area.Location.X >= _file.Width || Area.Location.Y >= _file.Height)
                {
                    Area = null;
                }

                Area.Width = width;
                Area.Height = height;
            
            }
            return _isActive;
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
            _isActive = true;
            _pointOfView = pointOfView;

            //we substract 1 to get area started from clicked pixel
            _initialLocation = new Point(location.X + pointOfView.X - 1, location.Y + pointOfView.Y - 1);
        }

        public void StopResizing()
        {
            _isActive = false;
        }

    }
}
