using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Abstract
{
    public class SurfaceFactory
    {
        public static Surface CreateSurface(RlViewer.Behaviors.PointSelector.PointSelector selector, Behaviors.ImageAligning.IRcsDependenceProvider rcsProvider)
        {
            switch (selector.Count())
            {
                case 3:
                    return new Concrete.Surface3Points(selector, rcsProvider);
                case 4:
                    return new Concrete.Surface4Points(selector, rcsProvider);
                case 16:
                    return new Concrete.Surface16Points(selector, rcsProvider);
                default:
                    break;
            }

            throw new NotSupportedException("Not supported points number");
        }


    }
}
