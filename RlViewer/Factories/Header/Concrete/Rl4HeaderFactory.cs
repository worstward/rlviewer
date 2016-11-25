using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Rl4;

namespace RlViewer.Factories.Header.Concrete
{
    class Rl4HeaderFactory : Abstract.HeaderFactory
    {
        public override Headers.Abstract.LocatorFileHeader Create(string path)
        {
            return new Rl4Header(path);
        }

        public override Headers.Abstract.LocatorFileHeader Create(Headers.Abstract.IHeaderStruct headerStruct)
        {
            return new Rl4Header(headerStruct);
        }
    }
}
