﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Factories.TileCreator.Abstract;
using RlViewer.Behaviors.TileCreator.Concrete;

namespace RlViewer.Factories.TileCreator.Concrete
{
    class Brl4TileCreatorFactory : TileCreatorFactory
    {
        public override RlViewer.Behaviors.TileCreator.Abstract.TileCreator Create(RlViewer.Files.LocatorFile locatorFile, Behaviors.TileCreator.TileOutputType type)
        {
            return new Brl4TileCreator(locatorFile, type);
        }

    }
}
