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
    public class TileDrawer : ImageDrawer
    {
        public TileDrawer(RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter, RlViewer.Behaviors.Scaling.Scaler scaler)
        {
            _filter = filter;
            _scaler = scaler;
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;
        private RlViewer.Behaviors.Scaling.Scaler _scaler;

        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                (int)(screenSize.Width / _scaler.ZoomFactor), (int)(screenSize.Height / _scaler.ZoomFactor))).ToArray();

            using (var g = Graphics.FromImage(canvas))
            {
                //g.ScaleTransform(1.0F, -1.0F);
                //g.TranslateTransform(0.0F, -(float)canvas.Height);
                Bitmap bmp;
                foreach (var tile in visibleTiles)
                {
                    bmp = new Bitmap(GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, Palette),
                        new Size((int)(tile.Size.Width * _scaler.ZoomFactor), (int)(tile.Size.Height * _scaler.ZoomFactor)));
                    g.DrawImage(bmp, new Point((int)((tile.LeftTopCoord.X - leftTopPointOfView.X) * _scaler.ZoomFactor),
                        (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) * _scaler.ZoomFactor)));
                }
            }
            return canvas;
        }
       

        /// <summary>
        /// Creates 8bpp image from raw byte array
        /// </summary>
        /// <param name="imgData">Raw image data</param>
        /// <param name="tileWidth">Image width</param>
        /// <param name="tileHeight">Image height</param>
        /// <returns>Grayscale image</returns>
        private Bitmap GetBmp(byte[] imgData, int tileWidth, int tileHeight, ColorPalette palette)
        {
            Bitmap bmp = new Bitmap(tileWidth, tileHeight, PixelFormat.Format8bppIndexed);
            bmp.Palette = palette;

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
