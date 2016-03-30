using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
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


        private Image ScaleDown(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY));
            int scale = (int)(1 / Scaler.ScaleFactor);
            int scalePower = (int)Math.Log(scale, 2);


            lock (_tileLocker)
            {
                using (var g = Graphics.FromImage(canvas))
                {
                    foreach (var tile in visibleTiles)
                    {
                        byte[] imgData = Tile.ReadData(tile.FilePath);
                        byte[] sievedImage = new byte[imgData.Length >> scalePower >> scalePower];

                        //int index = 0;

                        //for (int i = 0; i < tile.Size.Height * tile.Size.Width - tile.Size.Width; i += scale * tile.Size.Width)
                        //{
                        //    for (int j = i; j < i + tile.Size.Width; j += scale)
                        //    {
                        //        float cumulative = 0;
                        //        for (int k = j; k < j + scale; k++)
                        //        {
                        //            cumulative += imgData[k];
                        //        }

                        //        sievedImage[index] = (byte)(cumulative / scale);
                        //        index++;
                        //        cumulative = 0;
                        //    }
                        //}

                        Parallel.For(0, tile.Size.Height / scale, (i) =>
                        {
                            var localIndex = tile.Size.Width * i << scalePower;

                            for (int j = localIndex; j < localIndex + tile.Size.Width; j += scale)
                            {
                                float cumulative = 0;
                                for (int k = j; k < j + scale; k++)
                                {
                                    cumulative += imgData[k];
                                }

                                //(j & tile.Size.Width - 1) equals j % tile.Size.Width for powers of two (since tile.Size is power of 2 it's ok)
                                //bitwise version performs better
                                int index = i * (tile.Size.Width >> scalePower) + (j & tile.Size.Width - 1) / scale;
                                sievedImage[index] = (byte)(cumulative / scale);
                                cumulative = 0;
                            }
                        });

                        byte[] filteredData = _filter.ApplyFilters(sievedImage);

                        using (Bitmap tileImg = GetBmp(filteredData, tile.Size.Width / scale, tile.Size.Height / scale, Palette))
                        {
                            g.DrawImage(tileImg, new Point((int)((tile.LeftTopCoord.X - leftTopPointOfView.X) / scale),
                                (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) / scale)));
                        }

                    }
                }
            }
            return canvas;
        }


        //private Image ScaleDown(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        //{
        //    int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
        //    int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

        //    var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
        //        scaledScreenX, scaledScreenY));
        //    int scale = (int)(1 / Scaler.ScaleFactor);


        //    lock (_tileLocker)
        //    {
        //        using (var g = Graphics.FromImage(canvas))
        //        {
        //            foreach (var tile in visibleTiles)
        //            {
        //                byte[] imgData = Tile.ReadData(tile.FilePath);
        //                byte[] filteredData = _filter.ApplyFilters(imgData);
        //                Size scaledSize = new Size(tile.Size.Width / scale, tile.Size.Height / scale);

        //                using (Bitmap tileImg = GetBmp(filteredData, tile.Size.Width, tile.Size.Height, Palette))
        //                using (Bitmap resized = Resize(tileImg, scaledSize, InterpolationMode.High))
        //                {
        //                    g.DrawImage(resized, new Point((int)((tile.LeftTopCoord.X - leftTopPointOfView.X) / scale),
        //                        (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) / scale)));
        //                }

        //            }
        //        }
        //    }
        //    return canvas;
        //}

        private Image ScaleUp(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY)).ToArray();

            Point pointToDraw = new Point();

            
            //g.ScaleTransform(1.0F, -1.0F);
            //g.TranslateTransform(0.0F, -(float)canvas.Height);
            Size cropS = new Size();
            Tile leftTopTile = null;
            foreach (var tile in visibleTiles)
            {
                //stores relative offset by X for visible part from the beginning of the current tile
                int shiftTileX = (int)(leftTopPointOfView.X - tile.LeftTopCoord.X);
                shiftTileX = shiftTileX < 0 ? 0 : shiftTileX;

                //stores relative offset by Y for visible part from the beginning of the current tile
                int shiftTileY = (int)(leftTopPointOfView.Y - tile.LeftTopCoord.Y);
                shiftTileY = shiftTileY < 0 ? 0 : shiftTileY;

                //if not all scaled tile is visible we only take the visible part.
                int croppedWidth  = tile.Size.Width - shiftTileX >= scaledScreenX ? scaledScreenX : tile.Size.Width - shiftTileX;
                int croppedHeight = tile.Size.Height - shiftTileY >= scaledScreenY ? scaledScreenY : tile.Size.Height - shiftTileY;

                //determines resized canvas size
                Size resizedCanvasSize = new Size((int)(croppedWidth * Scaler.ScaleFactor), (int)(croppedHeight * Scaler.ScaleFactor));
                   
                //take lefttop visible tile to measure offset for other tiles
                if (tile == visibleTiles.First())
                {
                    leftTopTile = tile;
                    cropS = resizedCanvasSize;
                }

                //see pointToDraw description                    
                int x = tile.LeftTopCoord.X <= leftTopPointOfView.X ? 0 : cropS.Width  +
                    ((tile.LeftTopCoord.X - leftTopTile.LeftTopCoord.X) / tile.Size.Width - 1) * tile.Size.Width * (int)Scaler.ScaleFactor;
                int y = tile.LeftTopCoord.Y <= leftTopPointOfView.Y ? 0 : cropS.Height +
                    ((tile.LeftTopCoord.Y - leftTopTile.LeftTopCoord.Y) / tile.Size.Height - 1) * tile.Size.Height * (int)Scaler.ScaleFactor;


                //determines left top point on resized canvas to start drawing current tile from
                pointToDraw = new Point(x, y);
                lock (_tileLocker)
                {
                    using (var g = Graphics.FromImage(canvas))
                    {
                        byte[] imgData = Tile.ReadData(tile.FilePath);
                        byte[] filteredData = _filter.ApplyFilters(imgData);

                        using (Bitmap tileImg = GetBmp(filteredData, tile.Size.Width, tile.Size.Height, Palette))
                        using (Bitmap cropped = Crop(tileImg, shiftTileX, shiftTileY, croppedWidth, croppedHeight))
                        using (Bitmap resized = Resize(cropped, resizedCanvasSize, InterpolationMode.NearestNeighbor))
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

            lock (_tileLocker)
            {
                using (var g = Graphics.FromImage(canvas))
                {
                    foreach (var tile in visibleTiles)
                    {
                        using (Bitmap tileImg = GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, Palette))
                        {
                            int xToScreen = tile.LeftTopCoord.X - leftTopPointOfView.X;
                            int yToScreen = tile.LeftTopCoord.Y - leftTopPointOfView.Y;
                            g.DrawImage(tileImg, new Point(xToScreen, yToScreen));
                        }
                    
                    }
                }
            }
            return canvas;
        }


        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="canvas">Bitmap to draw on</param>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            if (Scaler.ScaleFactor == 1)
            {
                return ScaleNormal(canvas, tiles, leftTopPointOfView, screenSize);
            }
            else if (Scaler.ScaleFactor > 1)
            {
                return ScaleUp(canvas, tiles, leftTopPointOfView, screenSize);
            }
            else
            {
                return ScaleDown(canvas, tiles, leftTopPointOfView, screenSize);
            }
            
        }

        private Bitmap Crop(Bitmap bmp, int x, int y, int w, int h)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            return bmp.Clone(rect, bmp.PixelFormat);
        }

        private Bitmap Resize(Bitmap bmp, Size newSize, InterpolationMode mode)
        {
            Bitmap newBmp = new Bitmap(newSize.Width, newSize.Height);
            lock (_itemLocker)
            {
                using (var g = Graphics.FromImage(newBmp))
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.InterpolationMode = mode;
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
