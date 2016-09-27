using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.PointSharer.Concrete
{
    class RawPointSharerFactory : Abstract.PointSharerFactory
    {
        public override Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer Create(Files.LocatorFile file,
            Behaviors.CrossAppCommunication.ICrossAppExchange server, int guid)
        {
            var header = file.Header as Headers.Concrete.Brl4.Brl4Header;
            return new Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer(server, guid, 0, 0);
        }
    }
}
