using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using RlViewer.Behaviors.TileCreator;
using RlViewer.Files.Rli.Abstract;

namespace RlViewer.Behaviors.Draw
{

    /// <summary>
    /// Incapsulates image output functions
    /// </summary>
    public class Drawing
    {
        public Drawing(Size screenSize, RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter, PointSelector.PointSelector selector)
        {
            _filter = filter;
            _screenSize = screenSize;
            _selector = selector;
            _canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;
        private Bitmap _canvas;
        private Size _screenSize;
        private PointSelector.PointSelector _selector;
        private ColorPalette _gcp;
        
        private ColorPalette GrayPalette
        {
            get
            {
                return _gcp ?? InitGrayPalette();
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp grayscale image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitGrayPalette()
        {
            //TODO: REWRITE PALETTE INIT
            _gcp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;
            for (int i = 0; i < 256; i++)
                _gcp.Entries[i] = Color.FromArgb(255, i, i, i);
            return _gcp;
        }

        private object _canvasLocker = new object();

        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Bitmap DrawImage(Tile[] tiles, Point leftTopPointOfView)
        {
            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView, _screenSize.Width, _screenSize.Height));

            using (var g = Graphics.FromImage(_canvas))
            {
                foreach (var tile in visibleTiles)
                {
                    g.DrawImage(GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height),
                        new Point(tile.LeftTopCoord.X - leftTopPointOfView.X, tile.LeftTopCoord.Y - leftTopPointOfView.Y));
                }
                DrawPoints(g, new RectangleF(leftTopPointOfView, _screenSize));
            }
            return _canvas;
        }

        private void DrawPoints(Graphics g, RectangleF screen)
        {
            foreach (var point in _selector)
            {
                if (screen.Contains(point.Location))
                {
                    using(var pen = new Pen(Color.Red, 10f))
                    {
                        g.DrawRectangle(pen, new Rectangle((int)(point.Location.X - screen.Location.X), 
                            (int)(point.Location.Y - screen.Location.Y), 1, 1));
                    }           
                }
            }
        }


        /// <summary>
        /// Creates 8bpp grayscale image from raw byte array
        /// </summary>
        /// <param name="imgData">Raw image data</param>
        /// <param name="tileWidth">Image width</param>
        /// <param name="tileHeight">Image height</param>
        /// <returns>Grayscale image</returns>
        private Bitmap GetBmp(byte[] imgData, int tileWidth, int tileHeight)
        {
            Bitmap bmp = new Bitmap(tileWidth, tileHeight, PixelFormat.Format8bppIndexed);
            bmp.Palette = GrayPalette;

            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(imgData, 0, ptr, imgData.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
