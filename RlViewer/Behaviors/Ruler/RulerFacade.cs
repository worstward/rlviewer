using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer;
using RlViewer.Files;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Headers.Concrete.Brl4;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Headers.Concrete.R;
using System.Drawing;

namespace RlViewer.Behaviors.Ruler
{
    public class RulerFacade
    {

        public RulerFacade(LocatorFile file)
        {
            _file = file;
            ParseSteps(file);
            _ruler = new Ruler();
        }

        private LocatorFile _file;
        private Ruler _ruler;
        private float _dx;
        private float _dy;
        private Point _pt1;
        private Point _pt2;
        private bool _pt1Fixed;
        private bool _pt2Fixed;

        public bool Pt1Fixed
        {
            get
            {
                return _pt1Fixed;
            }
        }

        public bool Pt2Fixed
        {
            get
            {
                return _pt2Fixed;
            }
        }

        public Point Pt1
        {
            get
            {
                return _pt1;
            }
            set
            {
                _pt1Fixed = true;
                _pt2Fixed = false;
                _pt1 = value;
            }
        }

        public Point Pt2
        {
            get
            {
                return this._pt2;
            }
            set
            {
                _pt1Fixed = false;
                _pt2Fixed = true;
                _pt2 = value;
            }
        }


        private static int GetY(Point point1, Point point2, float x)
        {
            float k = (point2.Y - point1.Y) / (float)(point2.X - point1.X);
            float b = point1.Y - (k * point1.X);

            return (int)(k * x + b);
        }


        public string GetDistance(Point p2)
        {
            int x = p2.X;
            int y = p2.Y;

            if (p2.X > _file.Width)
            {
                x = _file.Width;
                y = GetY(Pt1, p2, _file.Width);
            }
            else if (x < 0)
            {
                x = 0;
            }

            if (y > _file.Height)
            {
                y = _file.Height;
            }
            else if (y < 0)
            {
                y = 0;
            }

            _pt2 = new Point(x, y);

            return GetDistance();
        }

        public void ResetRuler()
        {
            _pt1Fixed = false;
            _pt2Fixed = false;
        }

        public string GetDistance()
        {
            if (_dx == 0.0 || _dy == 0.0)
            {
                return string.Format("Расстояние: {0:0.##} пкс", _ruler.GetDistance(_pt1, _pt2));
            }
            else
            {
                return string.Format("Расстояние: {0:0.##} м", _ruler.GetDistance(_pt1, _pt2, _dx, _dy));
            }
        }

        private void ParseSteps(LocatorFile file)
        {
            switch (file.Properties.Type)
            {
                case FileType.brl4:
                    var brl4 = file as Brl4;
                    if (brl4 != null)
                    {
                        var brl4Header = brl4.Header as Brl4Header;
                        if (brl4Header != null)
                        {
                            _dx = brl4Header.HeaderStruct.rlParams.dx;
                            _dy = brl4Header.HeaderStruct.rlParams.dy;
                        }
                    }
                    break;
                case FileType.rl4:
                    var rl4 = file as Rl4;
                    if (rl4 != null)
                    {
                        var rl4Header = rl4.Header as Rl4Header;
                        if (rl4Header != null)
                        {
                            _dx = rl4Header.HeaderStruct.rlParams.dx;
                            _dy = rl4Header.HeaderStruct.rlParams.dy;
                        }
                    }
                    break;
                case FileType.r:
                    var r = file as R;
                    if (r != null)
                    {
                        var rHeader = r.Header as RHeader;
                        if (rHeader != null)
                        {
                            _dx = rHeader.HeaderStruct.synthesisHeader.dx;
                            _dy = rHeader.HeaderStruct.synthesisHeader.dy;
                        }
                    }
                    break;
            }
        }
    }
}
