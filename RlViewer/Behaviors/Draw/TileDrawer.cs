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
    /// Incapsulates image output functions
    /// </summary>
    public class TileDrawer : ImageDrawer
    {
        public TileDrawer(RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter, RlViewer.Behaviors.Scaling.Scaler scaler) : base(scaler)
        {
            _filter = filter;
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;


        private Image DrawWrappers(IEnumerable<TileImageWrapper> tilesToDraw, Size screenSize)
        {
            Bitmap canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);

            foreach (var t in tilesToDraw)
            {
                using (var g = Graphics.FromImage(canvas))
                {
                    g.DrawImage(t.TileImage, t.Location);
                }
            }

            return canvas;
        }


        private IEnumerable<TileImageWrapper> ScaleDown(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {

            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY));

            int scale = (int)(1 / Scaler.ScaleFactor);
            int scalePower = (int)Math.Log(scale, 2);
            
            BlockingCollection<TileImageWrapper> tileImgWrappers = new BlockingCollection<TileImageWrapper>();

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
                        //int cumulative = 0;
                        //for (int k = j; k < j + scale; k++)
                        //{
                        //    cumulative += imgData[k];
                        //}

                        sievedImage[index] = imgData[j];//(byte)(cumulative >> scalePower);
                        index++;
                    }
                }

                var tw = new TileImageWrapper(GetBmp(_filter.ApplyFilters(sievedImage),
                    tile.Size.Width >> scalePower, tile.Size.Height >> scalePower, Palette),
                    (int)((tile.LeftTopCoord.X - leftTopPointOfView.X) >> scalePower),
                    (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) >> scalePower));

                tileImgWrappers.Add(tw);
            });

            return tileImgWrappers;
        }

        
       //private Image ScaleDown(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
       //{
       //    int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
       //    int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

       //    var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
       //        scaledScreenX, scaledScreenY)).ToArray();

       //    int scale = (int)(1 / Scaler.ScaleFactor);
       //    int scalePower = (int)Math.Log(scale, 2);
           
       //    var defaultTile = tiles.First();
       //    var toInclusive = defaultTile.Size.Height * defaultTile.Size.Width - defaultTile.Size.Width;
       //    var step = defaultTile.Size.Width * scale;

       //    int scaleSq = scale * scale;

       //    var tileImgWrappers = new List<TileImageWrapper>(visibleTiles.Length);


       //    Parallel.ForEach(visibleTiles, tile =>
       //    {


       //        byte[] imgData = tile.ReadData();
       //        byte[] sievedImage = new byte[imgData.Length >> scalePower >> scalePower];

       //        //scale by averaging nearby pixels
       //        int index = 0;

       //        for (int i = 0; i < toInclusive; i += step)
       //        {
       //            for (int j = i; j < i + tile.Size.Width; j += scale)
       //            {
       //                //float cumulative = 0;
       //                //for (int n = j; n < j + tile.Size.Width * scale; n += tile.Size.Width)
       //                //{
       //                //    for (int k = n; k < n + scale; k++)
       //                //    {
       //                //        cumulative += imgData[k];
       //                //    }
       //                //}
       //                sievedImage[index++] = imgData[j];//(byte)(cumulative / scaleSq);
       //            }
       //        }

       //        byte[] filteredData = _filter.ApplyFilters(sievedImage);

       //        var tw = new TileImageWrapper(GetBmp(filteredData, tile.Size.Width / scale, tile.Size.Height / scale, Palette),
       //            new Point((int)((tile.LeftTopCoord.X - leftTopPointOfView.X) / scale),
       //                        (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) / scale)));

       //        tileImgWrappers.Add(tw);
       //    });

       //    foreach (var t in tileImgWrappers)
       //    {
       //        using (var g = Graphics.FromImage(canvas))
       //        {
       //            g.DrawImage(t.TileImage, t.Location);
       //        }
       //    }

       //    return canvas;
       //}


        private IEnumerable<TileImageWrapper> ScaleUp(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY)).ToArray();

            BlockingCollection<TileImageWrapper> tileImgWrappers = new BlockingCollection<TileImageWrapper>();
            
            //g.ScaleTransform(1.0F, -1.0F);
            //g.TranslateTransform(0.0F, -(float)canvas.Height)
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


                ////determines left top point on resized canvas to start drawing current tile from
                //pointToDraw = new Point(x, y);

                byte[] imgData = tile.ReadData();
                byte[] filteredData = _filter.ApplyFilters(imgData);

                using (Bitmap tileImg = GetBmp(filteredData, tile.Size.Width, tile.Size.Height, Palette))
                using (Bitmap cropped = Crop(tileImg, shiftTileX, shiftTileY, croppedWidth, croppedHeight))
                {
                    Bitmap resized = Resize(cropped, resizedCanvasSize, InterpolationMode.NearestNeighbor);
                    tileImgWrappers.Add(new TileImageWrapper(resized, x, y));
                }

            }


            return tileImgWrappers;
        }


        private IEnumerable<TileImageWrapper> ScaleNormal(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {

            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView,
                screenSize.Width, screenSize.Height));
            var tileImgWrappers = new List<TileImageWrapper>();

            foreach(var tile in visibleTiles)
            {
                Bitmap tileImg = GetBmp(_filter.ApplyFilters(tile.ReadData()), tile.Size.Width, tile.Size.Height, Palette);
                int xToScreen = tile.LeftTopCoord.X - leftTopPointOfView.X;
                int yToScreen = tile.LeftTopCoord.Y - leftTopPointOfView.Y;
                var tw = new TileImageWrapper(tileImg, xToScreen, yToScreen);
                tileImgWrappers.Add(tw);
            }

            return tileImgWrappers;
        }


        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="canvas">Bitmap to draw on</param>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(int width, int height, Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            IEnumerable<TileImageWrapper> wrappers;

            if (Scaler.ScaleFactor == 1)
            {
                wrappers =  ScaleNormal(tiles, leftTopPointOfView, screenSize);
            }
            else if (Scaler.ScaleFactor > 1)
            {
                wrappers = ScaleUp(tiles, leftTopPointOfView, screenSize);
            }
            else
            {
                wrappers = ScaleDown(tiles, leftTopPointOfView, screenSize);
            }

            return DrawWrappers(wrappers, screenSize);       
        }

        private Bitmap Crop(Bitmap bmp, int x, int y, int w, int h)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            return bmp.Clone(rect, bmp.PixelFormat);
        }

        private Bitmap Resize(Bitmap bmp, Size newSize, InterpolationMode mode)
        {
            Bitmap newBmp = new Bitmap(newSize.Width, newSize.Height);
            
            using (var g = Graphics.FromImage(newBmp))
            {
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = mode;
                g.DrawImage(bmp, 0, 0, newSize.Width, newSize.Height);
             
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
