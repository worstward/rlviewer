using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace RlViewer.Behaviors.PointSelector
{
    public class SelectedPoint
    {
        public SelectedPoint(RlViewer.Files.LocatorFile file, Point location, float rcs)
        {
            _location = location;
            _value = file.GetSample(location).ToFloatSample(file.Header.BytesPerSample);
            _rcs = rcs;
        }

        public SelectedPoint(Point location, float value, float rcs)
        {
            _location = location;
            _value = value;
            _rcs = rcs;
        }

        public SelectedPoint(RlViewer.Files.LocatorFile file, Point location)
        {      
            _location = location;
            _value = file.GetSample(location).ToFloatSample(file.Header.BytesPerSample);
        }


        private Point _location;

        /// <summary>
        /// X-Y point coordinates
        /// </summary>
        public Point Location
        {
            get { return _location; }
        }

        private float _value;
        /// <summary>
        /// Amplitude
        /// </summary>
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }


        private float _rcs;

        /// <summary>
        /// Radar cross-section
        /// </summary>
        public float Rcs
        {
            get { return _rcs; }
            set { _rcs = value; }
        }


    }
}
