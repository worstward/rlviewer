using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace RlViewer.Behaviors.Analyzing
{
    class PointAnalyzer
    {

        private bool _isMouseDown;

        private Point currentLocation; 

        public void StartTracing()
        {
            _isMouseDown = true;
        }

        public void StopTracing()
        {
            _isMouseDown = false;
        }



        public float Analyze(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            if (_isMouseDown)
            {
                if (location.X >= 0 && location.X < file.Width && location.Y >= 0 && location.Y < file.Height)
                {
                    if (currentLocation != location)
                    {
                        currentLocation = location;
                        return GetValue(file, location);
                    }
                }
            }
            return float.NaN;
            
        }

        private float GetValue(RlViewer.Files.LocatorFile file, Point p)
        {
            using (var fs = File.Open(file.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //TODO: provide complex samples support (2 float = 8 bytes)
                var offset = p.X * file.Header.BytesPerSample +
                    p.Y * (file.Width * file.Header.BytesPerSample + file.Header.StrHeaderLength) + file.Header.FileHeaderLength + file.Header.StrHeaderLength;
                fs.Seek(offset, SeekOrigin.Begin);
                var floatValue = new byte[file.Header.BytesPerSample];
                fs.Read(floatValue, 0, floatValue.Length);
                return BitConverter.ToSingle(floatValue, 0);
            }
        }



    }
}
