using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.NavigationSearcher.Abstract
{
    public abstract class PointFinderFactory
    {
        public abstract RlViewer.Behaviors.Navigation.Abstract.GeodesicPointFinder Create(RlViewer.Files.LocatorFile file);

        public static PointFinderFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Concrete.Brl4PointFinderFactory();
                case FileType.rl4:
                    return new Concrete.Rl4PointFinderFactory();
                case FileType.r:
                    return new Concrete.RPointFinderFactory();
                case FileType.raw:
                    return new Concrete.RawPointFinderFactory();
                case FileType.rl8:
                    return new Concrete.Rl8PointFinderFactory();

                default: throw new ArgumentException();
            }
        }

    }
}
