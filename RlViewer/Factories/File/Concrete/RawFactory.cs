using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Files.Rli.Concrete;
using RlViewer.Factories.File.Abstract;

namespace RlViewer.Factories.File.Concrete
{
    class RawFactory : FileFactory
    {
        public override LocatorFile Create(FileProperties properties,
            Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
        {
            return new Raw(properties, header, navi);
        }
   
    }
}
