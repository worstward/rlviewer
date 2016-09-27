using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.PointSharer.Concrete
{
    class Rl8PointSharerFactory : Abstract.PointSharerFactory
    {
        public override Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer Create(Files.LocatorFile file,
            Behaviors.CrossAppCommunication.ICrossAppExchange server, int guid)
        {
            var header = (Headers.Concrete.Rl8.Rl8Header)file.Header;
            return new Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer(server, guid,
                header.HeaderStruct.rlParams.sx, header.HeaderStruct.rlParams.sy);
        }
    }
}
