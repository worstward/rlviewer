using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    class Aligning
    {
        public Aligning(PointSelector.PointSelector selector)
        {
            _selector = selector;
        }


        PointSelector.PointSelector _selector;

    }
}
