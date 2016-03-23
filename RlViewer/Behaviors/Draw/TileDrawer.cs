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

        private object _tileLocker = new object();
        private object _itemLocker = new object();

        private Image ScaleDown()
        {
            return null;
        }

        private Image ScaleUp(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)(screenSize.Width / Scaler.ZoomFactor);
            int scaledScreenY = (int)(screenSize.Height / Scaler.ZoomFactor);

            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY)).ToArray();

            Point pointToDraw = new Point();

            using (var g = Graphics.FromImage(canvas))
            {
                //g.ScaleTransform(1.0F, -1.0F);
                //g.TranslateTransform(0.0F, -(float)canvas.Height);
                Size cropS = new Size();
                Tile leftTopTile = null;
                foreach (var tile in visibleTiles)
                {
                    int shiftTileX = (int)(leftTopPointOfView.X - tile.LeftTopCoord.X);
                    shiftTileX = shiftTileX < 0 ? 0 : shiftTileX;
                    if (shiftTileX >= tile.Size.Width) continue;

                    int shiftTileY = (int)(leftTopPointOfView.Y - tile.LeftTopCoord.Y);
                    shiftTileY = shiftTileY < 0 ? 0 : shiftTileY;
                    if (shiftTileY >= tile.Size.Height) continue;

                    int croppedWidth  = shiftTileX + scaledScreenX > tile.Size.Width  ? tile.Size.Width  - shiftTileX : scaledScreenX;
                    int croppedHeight = shiftTileY + scaledScreenY > tile.Size.Height ? tile.Size.Height - shiftTileY : scaledScreenY;

                    Size cropSize = new Size((int)((croppedWidth / (float)scaledScreenX) * screenSize.Width),
                        (int)((croppedHeight / (float)scaledScreenY) * screenSize.Height));


                    //take first visible tile to measure offset for other tiles
                    if (tile == visibleTiles.First())
                    {
                        leftTopTile = tile;
                        cropS = cropSize;
                    }

                    int x = tile.LeftTopCoord.X <= leftTopPointOfView.X ? 0 : cropS.Width  +
                        ((tile.LeftTopCoord.X - leftTopTile.LeftTopCoord.X) / tile.Size.Width - 1) * tile.Size.Width * (int)Scaler.ZoomFactor;
                    int y = tile.LeftTopCoord.Y <= leftTopPointOfView.Y ? 0 : cropS.Height +
                        ((tile.LeftTopCoord.Y - leftTopTile.LeftTopCoord.Y) / tile.Size.Height - 1) * tile.Size.Height * (int)Scaler.ZoomFactor;

                    pointToDraw = new Point(x, y);

                    using (Bitmap tileImg = GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, Palette))
                    using (Bitmap cropped = Crop(tileImg, shiftTileX, shiftTileY, croppedWidth, croppedHeight))
                    using (Bitmap resized = Resize(cropped, cropSize))
                    {
                        lock (_tileLocker)
                        {
                            g.DrawImage(resized, pointToDraw);
                        }
                    }

                }
            }
            return canvas;
        }

      


        private Image ScaleNormal(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                screenSize.Width, screenSize.Height)).ToArray();

            using (var g = Graphics.FromImage(canvas))
            {
                foreach (var tile in visibleTiles)
                {
                    using (Bitmap tileImg = GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, Palette))
                    {
                        lock (_tileLocker)
                        {
                            g.DrawImage(tileImg, new Point((int)((tile.LeftTopCoord.X - leftTopPointOfView.X)),
                                (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y))));
                        }
                    }
                }
            }
            return canvas;
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
            if (Scaler.ZoomFactor == 1)
            {
                return ScaleNormal(canvas, tiles, leftTopPointOfView, screenSize);
            }
            else if (Scaler.ZoomFactor > 1)
            {
                return ScaleUp(canvas, tiles, leftTopPointOfView, screenSize);
            }
            else
            {
                return ScaleNormal(canvas, tiles, leftTopPointOfView, screenSize);
            }
            
        }

        private Bitmap Crop(Bitmap bmp, int x, int y, int w, int h)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            return bmp.Clone(rect, bmp.PixelFormat);
        }

        private Bitmap Resize(Bitmap bmp, Size newSize)
        {
            Bitmap newBmp = new Bitmap(newSize.Width, newSize.Height);
            using (var g = Graphics.FromImage(newBmp))
            {
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                lock (_itemLocker)
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
