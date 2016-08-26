using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using RlViewer.Behaviors;
using RlViewer.Behaviors.Analyzing.Abstract;

namespace RlViewer.Behaviors.Analyzing.Concrete
{
    class FloatSampleAnalyzer : SampleAnalyzer
    {
        public override bool Analyze(RlViewer.Files.LocatorFile file, System.Drawing.Point location)
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
