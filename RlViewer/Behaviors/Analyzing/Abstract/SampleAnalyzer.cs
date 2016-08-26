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
        public abstract bool Analyze(Files.LocatorFile file, Point p);


    }
}
