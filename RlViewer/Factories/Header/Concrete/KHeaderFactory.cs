using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.K;

namespace RlViewer.Factories.Header.Concrete
{
    class KHeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new KHeader(path);
        }

        public override Headers.Abstract.LocatorFileHeader Create(Headers.Abstract.IHeaderStruct headerStruct)
        {
            return new KHeader(headerStruct);
        }
    }
}
