using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.NavigationChanger.Concrete
{
    class Brl4NavigationChangerFactory : Abstract.NavigationChangerFactory
    {
        public override Behaviors.Navigation.NavigationChanger.Abstract.NavigationChanger Create(Files.LocatorFile fileToChange, Files.LocatorFile sourceFile)
        {
            return new Behaviors.Navigation.NavigationChanger.Brl4NavigationChanger(fileToChange, sourceFile);
        }

    }
}
