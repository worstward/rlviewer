using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.ImageAligning.Surfaces.Abstract;
using RlViewer.Behaviors.ImageAligning.Surfaces.Concrete;

namespace RlViewer.Factories.Surface
{
    public class SurfaceFactory
    {
        public static Behaviors.ImageAligning.Surfaces.Abstract.Surface CreateSurface
            (RlViewer.Behaviors.PointSelector.PointSelector selector, Behaviors.ImageAligning.IInterpolationProvider rcsProvider)
        {
            switch (selector.Count())
            {
                case 3:
                    return new Surface3Points(selector, rcsProvider);
                case 4:
                    return new Surface4Points(selector, rcsProvider);
                case 16:
                    return new Surface16Points(selector, rcsProvider);
                default:
                    break;
            }

            throw new NotSupportedException("Not supported points number");
        }


    }
}
