using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Draw;

namespace RlViewer.Behaviors.TileCreator.Abstract
{
    public interface ITileCreator
    {
        Tile[] Tiles { get; }
    }
}
