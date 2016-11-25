using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Ba;

namespace RlViewer.Factories.Header.Concrete
{
    class BaHeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new BaHeader(path);
        }

        public override Headers.Abstract.LocatorFileHeader Create(Headers.Abstract.IHeaderStruct headerStruct)
        {
            throw new ArgumentException("headerStruct is invalid");
        }

    }
}
