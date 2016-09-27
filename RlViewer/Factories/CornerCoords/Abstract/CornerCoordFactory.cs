using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.CornerCoords.Abstract
{
    public abstract class CornerCoordFactory
    {
        public abstract RlViewer.Behaviors.ReportGenerator.CornerCoord.Abstract.CornerCoordinates Create(Files.LocatorFile file, int firstLine, int lastLine, bool readToEnd);
        public static CornerCoordFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Concrete.Brl4CornerCoordFactory();
                case FileType.k:
                    return new Concrete.KCornerCoordFactory();
                case FileType.raw:
                    return new Concrete.RawCornerCoordFactory();
                case FileType.rl4:
                    return new Concrete.Rl4CornerCoordFactory();
                case FileType.r:
                    return new Concrete.RCornerCoordFactory();
                case FileType.rl8:
                    return new Concrete.Rl8CornerCoordFactory();

                default: throw new ArgumentException();
            }
        }

    }
}
