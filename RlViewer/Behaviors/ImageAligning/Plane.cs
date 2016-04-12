using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    public class Plane
    {
        public Plane(PointSelector.PointSelector selector)
        {
            _selector = selector;
        }

        private PointSelector.PointSelector _selector;
    }
}
