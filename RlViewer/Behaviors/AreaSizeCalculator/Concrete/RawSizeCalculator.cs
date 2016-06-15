using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Concrete
{
    class RawSizeCalculator : RlViewer.Behaviors.AreaSizeCalculator.Abstract.SizeCalculator
    {
        public RawSizeCalculator(Files.LocatorFile file)
        {
            _dx = _dy = 1;
        }


        private float _dx;
        protected override float Dx
        {
            get { return _dx; }
        }


        private float _dy;
        protected override float Dy
        {
            get { return _dy; }
        }

    }
}
