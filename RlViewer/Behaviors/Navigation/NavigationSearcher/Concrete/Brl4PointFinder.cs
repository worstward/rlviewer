using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Navigation.NavigationSearcher.Concrete
{
    public class Brl4PointFinder : Abstract.GeodesicPointFinder
    {
        public Brl4PointFinder(Files.LocatorFile file) : base(file)
        {
            var header = file.Header as Headers.Concrete.Brl4.Brl4Header;
            NaviShift = header.HeaderStruct.rlParams.sx;
        }

    }
}
