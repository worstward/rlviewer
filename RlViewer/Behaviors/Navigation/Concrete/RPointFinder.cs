using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.Concrete
{
    class RPointFinder : Abstract.GeodesicPointFinder
    {
        public RPointFinder(Files.LocatorFile file)
            : base(file)
        {
            NaviShift = 0;
        }
    }
}
