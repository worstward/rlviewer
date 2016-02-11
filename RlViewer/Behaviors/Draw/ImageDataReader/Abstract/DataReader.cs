using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Draw.ImageDataReader.Abstract
{
    public abstract class DataReader
    {
        public abstract Tile[] Tiles { get; }

    }
}
