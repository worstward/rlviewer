using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.PointSharer.Concrete
{
    class Rl4PointSharerFactory : Abstract.PointSharerFactory
    {
        public override Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer Create(Files.LocatorFile file,
    System.Net.IPEndPoint multicastEp, int guid, Action<System.Drawing.Point> triggered)
        {
            var header = file.Header as Headers.Concrete.Rl4.Rl4Header;
            return new Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer(multicastEp, guid,
                header.HeaderStruct.rlParams.sx, header.HeaderStruct.rlParams.sy, triggered);
        }
    }
}
