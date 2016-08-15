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
            (Behaviors.PointSelector.CompressedPointSelectorWrapper selector,
            Behaviors.ImageAligning.IInterpolationProvider rcsProvider, bool useKriging)
        {
            var pointCount = selector.CompessedSelector.Count();

            if (useKriging && pointCount >= 3 && pointCount <= 16)
            {
                return new KrigingInterpolatedSurface(selector, rcsProvider);
            }

            switch (pointCount)
            {
                case 3:
                    return new Surface3Points(selector, rcsProvider);
                case 4:
                    return new Surface4Points(selector, rcsProvider);
                case 5:
                    return new Surface5Points(selector, rcsProvider);
                case 16:
                    return new Surface16Points(selector, rcsProvider);
                default:
                    break;
            }

            throw new NotSupportedException("Not supported points number");
        }


    }
}
