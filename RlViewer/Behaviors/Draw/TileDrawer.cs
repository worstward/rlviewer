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
using System.Collections.Concurrent;
using RlViewer.Behaviors.TileCreator;
using RlViewer.Files.Rli.Abstract;

namespace RlViewer.Behaviors.Draw
{

    /// <summary>
    /// Incapsulates tile output functions
    /// </summary>
    public class TileDrawer : ImageDrawer
    {
        public TileDrawer(RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter,
            RlViewer.Behaviors.Scaling.Scaler scaler) : base(scaler)
        {
            _filter = filter;
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;
        private IEnumerable<TileRawWrapper> _wrappers;


        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="canvas">Bitmap to draw on</param>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(int width, int height, Tile[] tiles, Point leftTopPointOfView, Size screenSize, bool highRes)
        {
           
            if (Scaler.ScaleFactor == 1)
            {
                _wrappers = ScaleNormal(tiles, leftTopPointOfView, screenSize);
            }
            else if (Scaler.ScaleFactor > 1)
            {
                _wrappers = ScaleUp(tiles, leftTopPointOfView, screenSize);
            }
            else
            {
                _wrappers = ScaleDown(tiles, leftTopPointOfView, screenSize, highRes);
            }

            return DrawWrappers(_wrappers, screenSize);
        }

        /// <summary>
        /// Redraws last fetched tiles
        /// </summary>
        /// <param name="screenSize">Current drawing area size</param>
        /// <returns></returns>
        public Image RedrawImage(Size screenSize)
        {
            if (_wrappers != null)
            {
                return DrawWrappers(_wrappers, screenSize);
            }
            else return null;
        }

        /// <summary>
        /// Draws imageWrapper with given image and its location
        /// </summary>
        /// <param name="tilesToDraw"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private Image DrawWrappers(IEnumerable<TileRawWrapper> tilesToDraw, Size screenSize)
        {
            Bitmap canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);

            var palette = _filter.ApplyColorFilters(Palette);

            using (var g = Graphics.FromImage(canvas))
            {
                foreach (var t in tilesToDraw)
                {
                    var bitmapToDraw = DrawingHelper.GetBmp(t.TileBytes, t.Width, t.Height, palette);
                    g.DrawImage(bitmapToDraw, t.Location);
                }
            }
            return canvas;
        }


        /// <summary>
        /// Draws imageWrapper with given byte buffer and its location
        /// </summary>
        /// <param name="tilesToDraw"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private Image DrawWrappers(IEnumerable<TileRawWrapper> tilesToDraw)
        {
            int width = 0;
            int height = 0;

            foreach (var item in tilesToDraw)
            {
                if (item.Location.X < 0)
                {
                    width += item.Location.X;
                }
                width += item.Width;

                if (item.Location.Y < 0)
                {
                    height += item.Location.Y;
                }
                height += item.Height;
            }

            var bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            foreach (var item in tilesToDraw)
            {
                var tileBmpData = new BitmapData();

                tileBmpData.Width = item.Location.X < 0 ? item.Location.X + item.Width : item.Width;
                tileBmpData.Height = item.Location.Y < 0 ? item.Location.Y + item.Height : item.Height;
                tileBmpData.PixelFormat = PixelFormat.Format8bppIndexed;
                tileBmpData.Stride = tileBmpData.Width;
                tileBmpData.Scan0 = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(item.TileBytes, 0);


                var location = new Point(item.Location.X, item.Location.Y);
                location.X = location.X < 0 ? 0 : location.X;
                location.Y = location.Y < 0 ? 0 : location.Y;

                BitmapData bmpData = bmp.LockBits(new Rectangle(location, new Size(tileBmpData.Width, tileBmpData.Height)),
                                            ImageLockMode.UserInputBuffer | ImageLockMode.WriteOnly,
                                            bmp.PixelFormat, tileBmpData);

                bmp.UnlockBits(bmpData);

              
            }

            return bmp;
        }



        /// <summary>
        /// Returns image from visible tiles with scale factor < 1
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="leftTopPointOfView"></param>
        /// <param name="screenSize"></param>
        /// <param name="highRes">Determines if algorithm uses averaging to get downscaled image (true if it uses)</param>
        /// <returns></returns>
        private IEnumerable<TileRawWrapper> ScaleDown(Tile[] tiles, Point leftTopPointOfView, Size screenSize, bool highRes)
        {

            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY));

            int scale = (int)(1 / Scaler.ScaleFactor);
            int scalePower = (int)Math.Log(scale, 2);
            
            BlockingCollection<TileRawWrapper> tileImgWrappers = new BlockingCollection<TileRawWrapper>();

            //var tileImgWrappers = new List<TileImageWrapper>(visibleTiles.Count());

            Parallel.ForEach(visibleTiles, tile =>
            {
                byte[] imgData = tile.ReadData();
                byte[] sievedImage = new byte[imgData.Length >> scalePower >> scalePower];

                //scale by averaging nearby pixels
                int index = 0;

                for (int i = 0; i < tile.Size.Height * tile.Size.Width - tile.Size.Width; i += scale * tile.Size.Width)
                {
                    for (int j = i; j < i + tile.Size.Width; j += scale)
                    {
                        if (highRes)
                        {
                            int cumulative = 0;
                            for (int k = j; k < j + scale; k++)
                            {
                                cumulative += imgData[k];
                            }
                            sievedImage[index] = (byte)(cumulative >> scalePower);
                        }
                        else
                        {
                            sievedImage[index] = imgData[j];
                        }

                        index++;
                    }
                }

                var tw = new TileRawWrapper(sievedImage,
                    (int)((tile.LeftTopCoord.X - leftTopPointOfView.X) >> scalePower),
                    (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) >> scalePower), 
                    tile.Size.Width >> scalePower, tile.Size.Height >> scalePower);

                tileImgWrappers.Add(tw);
            });

            return tileImgWrappers;
        }

        /// <summary>
        /// Returns image from visible tiles with scale factor > 1
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="leftTopPointOfView"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private IEnumerable<TileRawWrapper> ScaleUp(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY)).ToArray();

            BlockingCollection<TileRawWrapper> tileImgWrappers = new BlockingCollection<TileRawWrapper>();
            
            Size cropS = new Size();
            Tile leftTopTile = default(Tile);

            foreach(var tile in visibleTiles)
            {
                //stores relative offset by X for visible part from the beginning of the current tile
                int shiftTileX = (int)(leftTopPointOfView.X - tile.LeftTopCoord.X);
                shiftTileX = shiftTileX < 0 ? 0 : shiftTileX;

                //stores relative offset by Y for visible part from the beginning of the current tile
                int shiftTileY = (int)(leftTopPointOfView.Y - tile.LeftTopCoord.Y);
                shiftTileY = shiftTileY < 0 ? 0 : shiftTileY;

                //if not all scaled tile is visible we only take the visible part.
                int croppedWidth = tile.Size.Width - shiftTileX >= scaledScreenX ? scaledScreenX : tile.Size.Width - shiftTileX;
                int croppedHeight = tile.Size.Height - shiftTileY >= scaledScreenY ? scaledScreenY : tile.Size.Height - shiftTileY;

                var resizedW = (int)(croppedWidth * Scaler.ScaleFactor);
                var resizedH = (int)(croppedHeight * Scaler.ScaleFactor);

                var padding = (resizedW % 4);

                resizedW = resizedW + (padding == 0 ? 0 : 4 - padding);//bmp stride should be multiple of 4


                //determines resized canvas size
                Size resizedCanvasSize = new Size((int)(croppedWidth * Scaler.ScaleFactor), (int)(croppedHeight * Scaler.ScaleFactor));

                //take lefttop visible tile to measure offset for other tiles
                if (tile.Equals(visibleTiles.First()))
                {
                    leftTopTile = tile;
                    cropS = resizedCanvasSize;
                }

                //see pointToDraw description                    
                int x = tile.LeftTopCoord.X <= leftTopPointOfView.X ? 0 : cropS.Width +
                    ((tile.LeftTopCoord.X - leftTopTile.LeftTopCoord.X) / tile.Size.Width - 1) * tile.Size.Width * (int)Scaler.ScaleFactor;
                int y = tile.LeftTopCoord.Y <= leftTopPointOfView.Y ? 0 : cropS.Height +
                    ((tile.LeftTopCoord.Y - leftTopTile.LeftTopCoord.Y) / tile.Size.Height - 1) * tile.Size.Height * (int)Scaler.ScaleFactor;


                byte[] imgData = tile.ReadData();

                byte[] cropped = DrawingHelper.Crop(imgData, tile.Size.Width, shiftTileX, shiftTileY, croppedWidth, croppedHeight);

                byte[] resized = DrawingHelper.Resize(cropped, croppedWidth, resizedW, resizedH, Scaler.ScaleFactor);

                tileImgWrappers.Add(new TileRawWrapper(resized, x, y,
                    resizedCanvasSize.Width, resizedCanvasSize.Height));
            }


            return tileImgWrappers;
        }


        /// <summary>
        /// Returns image from visible tiles with 1:1 scale
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="leftTopPointOfView"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private IEnumerable<TileRawWrapper> ScaleNormal(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                screenSize.Width, screenSize.Height));
            var tileImgWrappers = new List<TileRawWrapper>();

            foreach(var tile in visibleTiles)
            {
                var tileBytes = tile.ReadData();
                int xToScreen = tile.LeftTopCoord.X - leftTopPointOfView.X;
                int yToScreen = tile.LeftTopCoord.Y - leftTopPointOfView.Y;
                tileImgWrappers.Add(new TileRawWrapper(tileBytes, xToScreen, yToScreen, tile.Size.Width, tile.Size.Height));
            }

            return tileImgWrappers;
        }

    }
}
