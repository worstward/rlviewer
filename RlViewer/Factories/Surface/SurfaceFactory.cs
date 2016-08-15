﻿using System;
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
            Behaviors.ImageAligning.IInterpolationProvider rcsProvider, 
            Behaviors.ImageAligning.Surfaces.SurfaceType surfaceType)
        {
            var pointCount = selector.CompessedSelector.Count();

            if (pointCount < 3 || pointCount > 16)
                throw new ArgumentException("Selected point count");

            switch (surfaceType)
            {
                case Behaviors.ImageAligning.Surfaces.SurfaceType.Kriging:
                    return new Behaviors.ImageAligning.Surfaces.Concrete.KrigingInterpolatedSurface(selector, rcsProvider);
                case Behaviors.ImageAligning.Surfaces.SurfaceType.RadicalBasisFunction:
                    return new Behaviors.ImageAligning.Surfaces.Concrete.RbfSurface(selector, rcsProvider);
                case Behaviors.ImageAligning.Surfaces.SurfaceType.Custom:
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
                            throw new NotSupportedException("Not supported points number");
                    }
                default:
                    throw new ArgumentException("Unknown surface type");
            }



        }


    }
}
