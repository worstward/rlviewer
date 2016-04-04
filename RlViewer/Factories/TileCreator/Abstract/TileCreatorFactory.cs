using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Factories.File.Concrete;
using RlViewer.Factories.TileCreator.Concrete;


namespace RlViewer.Factories.TileCreator.Abstract
{
    public abstract class TileCreatorFactory
    {
        public abstract RlViewer.Behaviors.TileCreator.Abstract.TileCreator Create(RlViewer.Files.LocatorFile rli);

        public static TileCreatorFactory GetFactory(FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Brl4TileCreatorFactory();
                case FileType.rl4:
                    return new Rl4TileCreatorFactory();
                case FileType.raw:
                    return new RawTileCreatorFactory();
                case FileType.r:
                    return new RTileCreatorFactory();
                case FileType.k:
                    throw new NotSupportedException("Implement me");
                default:
                    throw new NotSupportedException("Unsupported format");
            }
        }


    }
}
