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
        public TileDrawer(RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter, RlViewer.Behaviors.Scaling.Scaler scaler) : base(scaler)
        {
            _filter = filter;
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;

        object locker = new object();


        private Image ScaleDown()
        {
            return null;
        }

        private Image ScaleUp(Image canvas, Tile[] visibleTiles, Point leftTop, Size screen)
        {
            return null;
        }


        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)(screenSize.Width / Scaler.ZoomFactor);
            int scaledScreenY = (int)(screenSize.Height / Scaler.ZoomFactor);

            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY)).ToArray();

            int shiftTileX;
            int shiftTileY;
            Point pointToDraw = new Point();
            Size cropSize;

            using (var g = Graphics.FromImage(canvas))
            {
                //g.ScaleTransform(1.0F, -1.0F);
                //g.TranslateTransform(0.0F, -(float)canvas.Height);


                foreach (var tile in visibleTiles)
                {
                    shiftTileX = Scaler.ZoomFactor > 1 ? -(tile.LeftTopCoord.X - leftTopPointOfView.X) : 0;
                    shiftTileX = (int)Math.Abs(shiftTileX);

                    shiftTileY = Scaler.ZoomFactor > 1 ? -(tile.LeftTopCoord.Y - leftTopPointOfView.Y) : 0;
                    shiftTileY = (int)Math.Abs(shiftTileY);

                    pointToDraw = Scaler.ZoomFactor == 1 ? new Point((int)(tile.LeftTopCoord.X - leftTopPointOfView.X),
                       (int)(tile.LeftTopCoord.Y - leftTopPointOfView.Y)) : pointToDraw;

                    //int x = tile.LeftTopCoord.X <= leftTopPointOfView.X ? 0 : screenSize.Width - (leftTopPointOfView.X - tile.LeftTopCoord.X);
                    //int y = tile.LeftTopCoord.Y <= leftTopPointOfView.Y ? 0 : screenSize.Height - (leftTopPointOfView.Y - tile.LeftTopCoord.Y);
                    //pointToDraw = new Point(x, y);

                    cropSize = Scaler.ZoomFactor == 1 ? tile.Size : screenSize;
                    

                    
                    using (Bitmap tileImg = GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, Palette))
                    using (Bitmap cropped = Crop(tileImg, shiftTileX, shiftTileY,
                            (int)(cropSize.Width / Scaler.ZoomFactor), (int)(cropSize.Height / Scaler.ZoomFactor)))
                    using (Bitmap resized = Resize(cropped, cropSize))
                    {
                        lock (locker)
                        {
                            g.DrawImage(resized, pointToDraw);
                        }
                    }

                    //bmp = new Bitmap(GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, Palette),
                    //    new Size((int)(tile.Size.Width * _scaler.ZoomFactor), (int)(tile.Size.Height * _scaler.ZoomFactor)));

                    //g.DrawImage(bmp, new Point((int)((tile.LeftTopCoord.X - leftTopPointOfView.X)),
                    //   (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y))));
     
                }
            }
            return canvas;
        }

        private Bitmap Crop(Bitmap bmp, int x, int y, int w, int h)
        {
            int width = x + w > bmp.Width ? bmp.Width - x : w;
            int height = y + h > bmp.Height ? bmp.Height - y : h;

            Rectangle rect = new Rectangle(x, y, width, height);
            return bmp.Clone(rect, bmp.PixelFormat);
        }


        object L = new object();

        private Bitmap Resize(Bitmap bmp, Size newSize)
        {
            Bitmap newBmp = new Bitmap(newSize.Width, newSize.Height);
            using (var g = Graphics.FromImage(newBmp))
            {
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                lock (L)
                {
                    g.DrawImage(bmp, 0, 0, newSize.Width, newSize.Height);
                }
            }
            return newBmp;
            
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
