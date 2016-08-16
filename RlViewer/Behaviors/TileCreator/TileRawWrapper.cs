using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.TileCreator
{
    public class TileRawWrapper
    {

        public TileRawWrapper(byte[] bmpBytes, int x, int y, int width, int height)
        {
            TileBytes = bmpBytes;
            Location = new Point(x, y);
            Width = width;
            Height = height;
        }

        public TileRawWrapper(byte[] bmpBytes, Point location, Size tileSize)
        {
            TileBytes = bmpBytes;
            Location = location;
            Width = tileSize.Width;
            Height = tileSize.Height;
        }

        public byte[] TileBytes
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
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
