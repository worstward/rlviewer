using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Concrete
{
    class RSizeCalculator : RlViewer.Behaviors.AreaSizeCalculator.Abstract.SizeCalculator
    {
        public RSizeCalculator(Files.LocatorFile file)
        {
            var header = file.Header as Headers.Concrete.R.RHeader;
            _dx = header.HeaderStruct.synthesisHeader.dx;
            _dy = header.HeaderStruct.synthesisHeader.dy;
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
