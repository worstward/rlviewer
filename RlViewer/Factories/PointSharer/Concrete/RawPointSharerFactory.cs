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
    System.Net.IPEndPoint multicastEp, int guid, Action<System.Drawing.Point> triggered)
        {
            var header = file.Header as Headers.Concrete.Brl4.Brl4Header;
            return new Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer(multicastEp, guid, 0, 0, triggered);
        }
    }
}
