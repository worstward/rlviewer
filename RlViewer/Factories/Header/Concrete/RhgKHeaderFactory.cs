using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete;

namespace RlViewer.Factories.Header.Concrete
{
    class RhgKHeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new RhgKHeader(path);
        }
    }
}
