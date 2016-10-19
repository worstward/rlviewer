using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Analyzing
{
    public class SampleAnalyzer
    {

        protected bool _isMouseDown;
        protected Point _currentLocation;

        public void StartTracing()
        {
            _isMouseDown = true;
        }

        public void StopTracing()
        {
            _currentLocation = new Point(-1, -1);
            _isMouseDown = false;
        }


        /// <summary>
        /// Amplitude in current point
        /// </summary>
        public float Amplitude
        {
            get;
            protected set;
        }


        /// <summary>
        /// Gets amplitude in the provided point
        /// </summary>
        /// <param name="file">File to get amplitude from</param>
        /// <param name="p">Point to get amplitude at</param>
        /// <returns></returns>
        public virtual bool Analyze(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
        {
            bool hasLocationChanged = false;
            if (_isMouseDown)
            {
                if (location.X >= 0 && location.X < file.Width && location.Y >= 0 && location.Y < file.Height)
                {
                    if (_currentLocation != location)
                    {
                        _currentLocation = location;
                        hasLocationChanged = true;

                        try
                        {
                            Amplitude = file.GetSample(location).ToFileSample(file.Properties.Type, file.Header.BytesPerSample);
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
