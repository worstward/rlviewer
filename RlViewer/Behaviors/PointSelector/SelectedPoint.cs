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
        public SelectedPoint(Point location, float value)
        {
            _location = location;
            _value = value;
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
        }

        public float GetValue(RlViewer.Files.LocatorFile file, Point p)
        {
            using (var fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //TODO: provide complex samples support (2 float = 8 bytes)
                var offset = p.X * file.Header.BytesPerSample + 
                    p.Y * (file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength) + file.Header.FileHeaderLength;
                fs.Seek(offset, SeekOrigin.Begin);
                var floatValue = new byte[file.Header.BytesPerSample];
                fs.Read(floatValue, 0, floatValue.Length);
                return BitConverter.ToSingle(floatValue, 0);
            }
        }

    }
}
