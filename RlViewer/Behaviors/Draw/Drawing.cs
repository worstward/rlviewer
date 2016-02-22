using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Drawing(Tile[] tiles, Size screenSize)
        {
            _tiles = tiles;
            _canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);
        }

        private Bitmap _canvas;
        private Tile[] _tiles;

        private ColorPalette gcp;

        private ColorPalette GrayPalette
        {
            get
            {
                return gcp == null ? InitGrayPalette() : gcp;
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp grayscale image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitGrayPalette()
        {
            //TODO: REWRITE PALETTE INIT
            gcp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;
            for (int i = 0; i < 256; i++)
                gcp.Entries[i] = Color.FromArgb(255, i, i, i);
            return gcp;
        }

        
        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>
        public Bitmap Draw(Size screenSize, Tile[] tiles, Point leftTopPointOfView)
        {
            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView, screenSize.Width, screenSize.Height) == true);

            using (var g = Graphics.FromImage(_canvas))
            {
                foreach (var tile in visibleTiles)
                {
                    g.DrawImage(GetBmp(Tile.ReadData(tile.FilePath), tile.Size.Width, tile.Size.Height),
                        new Point(tile.LeftTopCoord.X - leftTopPointOfView.X, tile.LeftTopCoord.Y - leftTopPointOfView.Y));
                }
                
#if DEBUG
                //bmp.Save("a.bmp");
#endif
            }
            return _canvas;
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

        //RlViewer.Behaviors.TileCreator.Abstract.TileCreator _tileCreator;

       // public abstract Tile[] Tiles { get; }
    }
}
