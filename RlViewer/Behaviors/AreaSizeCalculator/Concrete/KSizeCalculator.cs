using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Concrete
{
    class KSizeCalculator : RlViewer.Behaviors.AreaSizeCalculator.Abstract.SizeCalculator
    {
        public KSizeCalculator(Headers.Abstract.LocatorFileHeader header)
        {
            var fileHead = header as Headers.Concrete.K.KHeader;
            _dx = 300 / (2 * fileHead.HeaderStruct.adcHeader.adcFreq);
            _dy = _dx;
            //_freq = fileHead.HeaderStruct.adcHeader.adcFreq;
            //_impulseLength = fileHead.HeaderStruct

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

        //private float _freq;
        //protected override float Frequency
        //{
        //    get
        //    {
        //        return _freq;
        //    }
        //}

        //private float _impulseLength;
        //protected override float ImpulseLength
        //{
        //    get
        //    {
        //        return _impulseLength;
        //    }
        //}
    }
}
