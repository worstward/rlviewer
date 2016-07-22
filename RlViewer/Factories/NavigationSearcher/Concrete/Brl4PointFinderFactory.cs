using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.NavigationSearcher.Concrete
{
    public class Brl4PointFinderFactory : Abstract.PointFinderFactory
    {
        public override Behaviors.Navigation.NavigationSearcher.Abstract.GeodesicPointFinder Create(Files.LocatorFile file)
        {
            return new Behaviors.Navigation.NavigationSearcher.Concrete.Brl4PointFinder(file);
        } 
    }
}
