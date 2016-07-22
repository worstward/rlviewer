using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.NavigationSearcher.Concrete
{
    class KPointFinder : Abstract.GeodesicPointFinder
    {
        public KPointFinder(Files.LocatorFile file)
            : base(file)
        {
            NaviShift = 0;
        }
    }
}
