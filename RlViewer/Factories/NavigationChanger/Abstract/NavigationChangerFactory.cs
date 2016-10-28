using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.NavigationChanger.Abstract
{
    public abstract class NavigationChangerFactory
    {
        public abstract Behaviors.Navigation.NavigationChanger.Abstract.NavigationChanger Create(Files.LocatorFile fileToChange, Files.LocatorFile sourceFile);

        public static Factories.NavigationChanger.Abstract.NavigationChangerFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.rl4:
                    return new Factories.NavigationChanger.Concrete.Rl4NavigationChangerFactory();
                case FileType.brl4:
                    return new Factories.NavigationChanger.Concrete.Brl4NavigationChangerFactory();
                default:
                    throw new ArgumentException("Navigation changer type");
            }
        }
    }
}
