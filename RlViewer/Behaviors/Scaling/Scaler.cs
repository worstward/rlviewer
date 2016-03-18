using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Scaling
{
    public class Scaler
    {
        public Scaler(float zoomFactor)
        {
            _zoomFactor = zoomFactor;
        }

        private float _zoomFactor;
        public float ZoomFactor
        {
            get { return _zoomFactor; }
        }

    }
}
