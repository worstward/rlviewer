using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.TileCreator
{
    public class TileImageWrapper
    {
        public TileImageWrapper(Bitmap bmp, Point location)
        {
            TileImage = bmp;
            Location = location;
        }

        public Bitmap TileImage
        {
            get;
            private set;
        }

        public Point Location
        {
            get;
            private set;
        }


    }
}
