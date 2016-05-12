using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.NavigationSearcher.Concrete
{
    class Rl4PointFinderFactory : Abstract.PointFinderFactory
    {
        public override Behaviors.Navigation.Abstract.GeodesicPointFinder Create(Files.LocatorFile file)
        {
            return new Behaviors.Navigation.Concrete.Rl4PointFinder(file);
        }
    }
}
