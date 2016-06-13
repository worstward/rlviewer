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
        public abstract RlViewer.Behaviors.TileCreator.Abstract.ITileCreator Create(RlViewer.Files.LocatorFile rli, Behaviors.TileCreator.TileOutputType type);

        public static TileCreatorFactory GetFactory(LocatorFile file)
        {
            switch (file.Properties.Type)
            {
                case FileType.brl4:
                    return new Brl4TileCreatorFactory();
                case FileType.rl4:
                    return new Rl4TileCreatorFactory();
                case FileType.raw:
                    if (file.Header.BytesPerSample == 4)
                    {
                        return new Raw4TileCreatorFactory();
                    }
                    else
                    {
                        return new Raw8TileCreatorFactory();
                    }                    
                case FileType.r:
                    return new RTileCreatorFactory();
                case FileType.rl8:
                    return new Rl8TileCreatorFactory();
                case FileType.k:
                    return new KTileCreatorFactory();
                default:
                    throw new NotSupportedException("Unsupported format");
            }
        }


    }
}
