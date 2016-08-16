using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.TileCreator
{
    class TileImgWrapper
    {
        public TileImgWrapper(Bitmap bmp, Point location)
        {
            TileImage = bmp;
            Location = location;
        }

        public TileImgWrapper(Bitmap bmp, int x, int y)
        {
            TileImage = bmp;
            Location = new Point(x, y);
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
