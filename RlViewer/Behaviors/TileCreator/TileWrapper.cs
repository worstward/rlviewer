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

        public TileImageWrapper(Bitmap bmp, int x, int y)
        {
            TileImage = bmp;
            Location = new Point(x, y);
        }

        public TileImageWrapper(byte[] bmpData, Point location, int width, int height)
        {
            TileBytes = bmpData;
            Location = location;
            Width = width;
            Height = height;
        }

        public TileImageWrapper(byte[] bmpData, int x, int y, int width, int height)
        {
            TileBytes = bmpData;
            Location = new Point(x, y);
            Width = width;
            Height = height;
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


        public byte[] TileBytes
        {
            get;
            private set;
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
