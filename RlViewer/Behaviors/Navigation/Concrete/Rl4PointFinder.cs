using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.Concrete
{
    public class Rl4PointFinder : Abstract.GeodesicPointFinder
    {
        public Rl4PointFinder(Files.LocatorFile file)
            : base(file)
        {
            var header = file.Header as Headers.Concrete.Rl4.Rl4Header;
            NaviShift = header.HeaderStruct.rlParams.sx;
        }

    }
}
