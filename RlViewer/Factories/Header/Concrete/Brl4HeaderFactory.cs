using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Brl4;

namespace RlViewer.Factories.Header.Concrete
{
    class Brl4HeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new Brl4Header(path);
        }

    }
}
