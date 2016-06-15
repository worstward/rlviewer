using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Concrete
{
    class KSizeCalculator : RlViewer.Behaviors.AreaSizeCalculator.Abstract.SizeCalculator
    {
        public KSizeCalculator(Files.LocatorFile file)
        {
            var header = file.Header as Headers.Concrete.K.KHeader;
            _dx = 300 / (2 * header.HeaderStruct.adcHeader.adcFreq);
            _dy = _dx;
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
