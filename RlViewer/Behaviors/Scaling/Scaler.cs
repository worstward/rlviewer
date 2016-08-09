using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Scaling
{
    public class Scaler
    {
        public Scaler(float minZoom, float maxZoom, float scaleFactor = 1)
        {
            MinZoom = minZoom;
            MaxZoom = maxZoom;
            ScaleFactor = scaleFactor;
        }

        private float _scaleFactor;
        public float ScaleFactor
        {
            get
            {
                return _scaleFactor; 
            }
            private set 
            {
                var factor = value <= MaxZoom ? value : MaxZoom;
                factor = factor > MinZoom ? factor : MinZoom;
                _scaleFactor = factor;
            }
        }


        public float MinZoom
        {
            get;
            private set;
        }

        public float MaxZoom
        {
            get;
            private set;
        }
    }
}
