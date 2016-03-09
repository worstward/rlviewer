﻿using System;
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
    public class TileDrawer
    {
        public TileDrawer(RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter)
        {
            _filter = filter;
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;

        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(Image canvas, Tile[] tiles, Point leftTopPointOfView, Size screenSize, ColorPalette palette)
        {
            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView, screenSize.Width, screenSize.Height)).ToArray();

            //var img = (Image)_canvas.Clone();
            using (var g = Graphics.FromImage(canvas))
            {
                foreach (var tile in visibleTiles)
                {
                    g.DrawImage(GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height, palette),
                        new Point(tile.LeftTopCoord.X - leftTopPointOfView.X, tile.LeftTopCoord.Y - leftTopPointOfView.Y));
                }
            }
            return canvas;
        }
       

        /// <summary>
        /// Creates 8bpp grayscale image from raw byte array
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
