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
            currentLocation = new Point(-1, -1);
            _isMouseDown = false;
        }

        public float Amplitude
        {
            get;
            private set;
        }
  

        public bool Analyze(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            bool hasLocationChanged = false;
            if (_isMouseDown)
            {
                if (location.X >= 0 && location.X < file.Width && location.Y >= 0 && location.Y < file.Height)
                {
                    if (currentLocation != location)
                    {
                        currentLocation = location;
                        hasLocationChanged = true;

                        try
                        {
                            Amplitude = FileReader.GetSample(file, location);
                        }
                        catch (Exception)
                        {
                            _isMouseDown = false;
                            throw;
                        }
                    }
                }
            }

            return hasLocationChanged;
                     
        }



    }
}
