using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Behaviors.TileCreator.Concrete;
using RlViewer.Behaviors.TileCreator.Abstract;


namespace RlViewer.Factories.TileCreator.Concrete
{
    class RawTileCreatorFactory : TileCreatorFactory
    {
        public override ITileCreator Create(RlViewer.Files.LocatorFile locatorFile)
        {
            return new RawTileCreator(locatorFile);
        }
    }
}
