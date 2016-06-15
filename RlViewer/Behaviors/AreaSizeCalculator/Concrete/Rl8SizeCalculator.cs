using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Concrete
{
    class Rl8SizeCalculator : RlViewer.Behaviors.AreaSizeCalculator.Abstract.SizeCalculator
    {
        public Rl8SizeCalculator(Files.LocatorFile file)
        {
            var header = file.Header as Headers.Concrete.Rl8.Rl8Header;
            _dx = header.HeaderStruct.rlParams.dx;
            _dy = header.HeaderStruct.rlParams.dy;
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
