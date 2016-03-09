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
        public Drawing(Size screenSize, RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter, 
            PointSelector.PointSelector pointSelector, AreaSelector.AreaSelector areaSelector)
        {
            _filter = filter;
            _screenSize = screenSize;
            _pointSelector = pointSelector;
            _areaSelector = areaSelector;
            _canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);

        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;
        private Image _canvas;

        public Image Canvas
        {
            get { return _canvas; }
        }


        private Size _screenSize;
        private PointSelector.PointSelector _pointSelector;
        private AreaSelector.AreaSelector _areaSelector;
        private ColorPalette _colorPalette;
        
        private ColorPalette Palette
        {
            get
            {
                return _colorPalette ?? InitPalette();
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitPalette()
        {
            //TODO: REWRITE PALETTE INIT
            _colorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;
            if (PaletteParams.Reversed)
            {
                for (int i = 0; i < 256; i++)
                {
                    var r = PaletteParams.R * i;
                    r = r > 255 ? 255 : r;

                    var g = PaletteParams.G * i;
                    g = g > 255 ? 255 : g;

                    var b = PaletteParams.B * i;
                    b = b > 255 ? 255 : b;

                    _colorPalette.Entries[255 - i] = Color.FromArgb(255, r, g, b);
                }     
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    var r = PaletteParams.R * i;
                    r = r > 255 ? 255 : r;

                    var g = PaletteParams.G * i;
                    g = g > 255 ? 255 : g;

                    var b = PaletteParams.B * i;
                    b = b > 255 ? 255 : b;

                    _colorPalette.Entries[i] = Color.FromArgb(255, r, g, b);
                }  
            }
             return _colorPalette;
        }

        /// <summary>
        /// Changes palette color table according to input RGB values
        /// </summary>
        /// <param name="R">Red channel</param>
        /// <param name="G">Green channel</param>
        /// <param name="B">Blue channel</param>
        /// <param name="reversed">Determines if colors in color table are reversed</param>
        public void GetPalette(int R, int G, int B, bool reversed)
        {
            _colorPalette = null;
            PaletteParams.R = R;
            PaletteParams.G = G;
            PaletteParams.B = B;
            PaletteParams.Reversed = reversed;
        }




        private object _canvasLocker = new object();

        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(Tile[] tiles, Point leftTopPointOfView)
        {
            var visibleTiles = tiles.AsParallel().Where(x => x.CheckVisibility(leftTopPointOfView, _screenSize.Width, _screenSize.Height)).ToArray();

            //var img = (Image)_canvas.Clone();
            using (var g = Graphics.FromImage(_canvas))
            {
                foreach (var tile in visibleTiles)
                {
                    g.DrawImage(GetBmp(_filter.ApplyFilters(Tile.ReadData(tile.FilePath)), tile.Size.Width, tile.Size.Height),
                        new Point(tile.LeftTopCoord.X - leftTopPointOfView.X, tile.LeftTopCoord.Y - leftTopPointOfView.Y));
                }
            }
            return DrawImage(_canvas, leftTopPointOfView);
        }

        public Image DrawImage(Image canvas, Point leftTopPointOfView)
        {
            return DrawItems(canvas, leftTopPointOfView);
        }


        private Image DrawItems(Image canvas, Point leftTopPointOfView)
        {
            GC.Collect();
            var screen = new RectangleF(leftTopPointOfView, _screenSize);
            var img = (Image)canvas.Clone();
            using (var g = Graphics.FromImage(img))
            {
                DrawPoints(g, screen);
                DrawArea(g, screen);
            }
            return img;
        }



        private void DrawPoints(Graphics g, RectangleF screen)
        {
            foreach (var point in _pointSelector)
            {
                if (screen.Contains(point.Location))
                {
                    using(var pen = new Pen(Color.Red, 3f))
                    {
                        g.DrawRectangle(pen, new Rectangle((int)(point.Location.X - screen.Location.X), 
                            (int)(point.Location.Y - screen.Location.Y), 1, 1));
                    }           
                }
            }
        }

        private void DrawArea(Graphics g, RectangleF screen)
        {
            using(var pen = new Pen(Palette.Entries[200]) {DashPattern = new float[] {5, 2, 15, 4}})
            {
                g.DrawRectangle(pen, (int)(_areaSelector.Area.Location.X - screen.X), (int)(_areaSelector.Area.Location.Y - screen.Y),
                    _areaSelector.Area.Width, _areaSelector.Area.Height);
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
            bmp.Palette = Palette;

            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);
            
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(imgData, 0, ptr, imgData.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }


        private static class PaletteParams
        {

            private static bool _reversed;

            public static bool Reversed
            {
                get
                {
                    return _reversed;
                }
                set
                {
                    _reversed = value;
                }
            }



            private static int _red = 1;
            public static int R
            {
                get
                {
                    return _red;
                }
                set
                {
                    _red = value < 1 ? 0 : value;
                }
            }

            private static int _green = 1;
            public static int G
            {
                get
                {
                    return _green;
                }
                set
                {
                    _green = value < 1 ? 0 : value;
                }
            }

            private static int _blue = 1;
            public static int B
            {
                get
                {
                    return _blue;
                }
                set
                {
                    _blue = value < 1 ? 0 : value;
                }
            }
        }


    }
}
