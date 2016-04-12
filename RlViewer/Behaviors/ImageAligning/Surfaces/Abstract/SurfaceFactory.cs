using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Abstract
{
    public class SurfaceFactory
    {
        public static Surface CreateSurface(RlViewer.Behaviors.PointSelector.PointSelector selector)
        {
            switch (selector.Count())
            {
                case 3:
                    return new Concrete.Surface3Points(selector);
                case 16:
                    return new Concrete.Surface16Points(selector);
                default:
                    break;
            }

            throw new NotSupportedException("Not supported points number");
        }


    }
}
