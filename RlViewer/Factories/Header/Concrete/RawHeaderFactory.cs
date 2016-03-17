using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Raw;

namespace RlViewer.Factories.Header.Concrete
{
    class RawHeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new RawHeader(path);
        }
    }
}
