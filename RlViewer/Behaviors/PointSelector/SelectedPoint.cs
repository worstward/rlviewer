using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace RlViewer.Behaviors.PointSelector
{
    public class SelectedPoint : PointReader
    {
        public SelectedPoint(RlViewer.Files.LocatorFile file, Point location, float epr)
        {
            _location = location;
            _value = GetValue(file, location);
            _epr = epr;
        }

        public SelectedPoint(RlViewer.Files.LocatorFile file, Point location)
        {
            _location = location;
            _value = GetValue(file, location);
        }

        private Point _location;

        public Point Location
        {
            get { return _location; }
        }

        private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }


        private float _epr;
        public float Epr
        {
            get { return _epr; }
            set { _epr = value; }
        }


    }
}
