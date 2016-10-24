using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    public class NormalizingPlane : Surfaces.Abstract.Surface
    {
        public NormalizingPlane(PointSelector.CompressedPointSelectorWrapper selector)
            : base(selector)
        { }


        protected override IInterpolationProvider RcsProvider
        {
            get 
            { 
                throw new NotImplementedException();
            }
        }

        public override byte[] ResampleImage(Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            throw new NotImplementedException();
        }
    }
}
