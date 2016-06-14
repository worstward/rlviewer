using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Analyzing.Abstract
{
    public abstract class SampleAnalyzer
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

        public float Amplitude
        {
            get;
            protected set;
        }

        public abstract bool Analyze(Files.LocatorFile file, Point p);


    }
}
