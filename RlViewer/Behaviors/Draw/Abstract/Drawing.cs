using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Files.Rli.Abstract;

namespace RlViewer.Behaviors.Draw.Abstract
{
    abstract class Drawing
    {
        public Drawing(RliFile rli)
        {
            
        }

        public abstract Tile[] Tiles { get; }


    }
}
