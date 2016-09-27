using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.PointSharer.Abstract
{
    public abstract class PointSharerFactory
    {
        public abstract RlViewer.Behaviors.CrossAppCommunication.PointSharer.MulticastPointSharer Create(Files.LocatorFile file,
            Behaviors.CrossAppCommunication.ICrossAppExchange server, int guid);

        public static PointSharerFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Concrete.Brl4PointSharerFactory();
                case FileType.k:
                    return new Concrete.KPointSharerFactory();
                case FileType.raw:
                    return new Concrete.RawPointSharerFactory();
                case FileType.rl4:
                    return new Concrete.Rl4PointSharerFactory();
                case FileType.r:
                    return new Concrete.RPointSharerFactory();
                case FileType.rl8:
                    return new Concrete.Rl8PointSharerFactory();

                default: throw new ArgumentException();
            }
        }

    }
}
