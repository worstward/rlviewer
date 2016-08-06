using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.PointSharer.Concrete
{
    class RPointSharerFactory : Abstract.PointSharerFactory
    {
        public override Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer Create(Files.LocatorFile file,
    System.Net.IPEndPoint multicastEp, int guid, Action<System.Drawing.Point> triggered)
        {
            return new Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer(multicastEp, guid, 0, 0 , triggered);
        }
    }
}
