using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.R;

namespace RlViewer.Factories.Header.Concrete
{
    class RHeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new RHeader(path);
        }

        public override Headers.Abstract.LocatorFileHeader Create(Headers.Abstract.IHeaderStruct headerStruct)
        {
            return new RHeader(headerStruct);
        }
    }
}
