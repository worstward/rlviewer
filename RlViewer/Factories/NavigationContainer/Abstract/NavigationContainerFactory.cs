using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.NavigationContainer.Abstract
{
    public abstract class NavigationContainerFactory
    {
        public abstract RlViewer.Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header);

        public static NavigationContainerFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Concrete.Brl4NavigationContainerFactory();
                case FileType.k:
                    return new Concrete.KNavigationContainerFactory();
                case FileType.raw:
                    return new Concrete.RawNavigationContainerFactory();
                case FileType.rl4:
                    return new Concrete.Rl4NavigationContainerFactory();
                case FileType.r:
                    return new Concrete.RNavigationContainerFactory();
                case FileType.rl8:
                    return new Concrete.Rl8NavigationContainerFactory();
                case FileType.ba:
                    return new Concrete.BaNavigationContainerFactory();
                default: throw new ArgumentException();
            }
        }

    }
}
