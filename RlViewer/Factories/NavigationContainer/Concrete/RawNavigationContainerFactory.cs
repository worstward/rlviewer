using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    class RawNavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            return new RawNavigationContainer(properties.FilePath);
        }
    }
}
