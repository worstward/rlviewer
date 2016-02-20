﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Factories.File.Concrete;
using RlViewer.Factories.TileCreator.Concrete;
using RlViewer.Behaviors.TileCreator.Abstract;


namespace RlViewer.Factories.TileCreator.Abstract
{
    public abstract class TileCreatorFactory
    {
        public abstract ITileCreator Create(RlViewer.Files.LocatorFile rli);

        public static TileCreatorFactory GetFactory(FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    throw new NotSupportedException("Implement me");
                case FileType.rl4:
                    return new Rl4TileCreatorFactory();
                case FileType.raw:
                    return new RawTileCreatorFactory() ;
                case FileType.k:
                    throw new NotSupportedException("Implement me");
                default:
                    throw new NotSupportedException("Unsupported format");
            }
        }


    }
}
