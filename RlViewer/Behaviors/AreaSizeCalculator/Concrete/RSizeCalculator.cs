using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Concrete
{
    class RSizeCalculator : RlViewer.Behaviors.AreaSizeCalculator.Abstract.SizeCalculator
    {
        public RSizeCalculator(Headers.Abstract.LocatorFileHeader header)
        {
            var fileHead = header as Headers.Concrete.R.RHeader;
            _dx = fileHead.HeaderStruct.synthesisHeader.dx;
            _dy = fileHead.HeaderStruct.synthesisHeader.dy * fileHead.HeaderStruct.synthesisHeader.MScale;
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
