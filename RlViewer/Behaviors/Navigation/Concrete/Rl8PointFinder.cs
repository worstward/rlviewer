using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.Concrete
{
    class Rl8PointFinder : Abstract.GeodesicPointFinder
    {
        public Rl8PointFinder(Files.LocatorFile file)
            : base(file)
        {
            var header = file.Header as Headers.Concrete.Rl8.Rl8Header;
            NaviShift = header.HeaderStruct.rlParams.sx;
        }

    }
}
