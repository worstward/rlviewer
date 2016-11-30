﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Behaviors.TileCreator.Concrete;

namespace RlViewer.Factories.TileCreator.Concrete
{
    class KTileCreatorFactory : TileCreatorFactory
    {
        public override RlViewer.Behaviors.TileCreator.Abstract.ITileCreator Create(RlViewer.Files.LocatorFile locatorFile,
            Behaviors.TileCreator.TileOutputType type, System.Drawing.Size tileSize)
        {
            return new KTileCreator(locatorFile, type, tileSize);
        }
        
    }
}
