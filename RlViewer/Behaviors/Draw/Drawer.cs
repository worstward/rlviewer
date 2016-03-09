using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace RlViewer.Behaviors.Draw
{
    class Drawer
    {
        public Drawer(Size screenSize, ItemDrawer iDrawer, TileDrawer tDrawer)
        {
            _screenSize = screenSize;
            _iDrawer = iDrawer;
            _tDrawer = tDrawer;
            _canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);
        }

        private ItemDrawer _iDrawer;
        private TileDrawer _tDrawer;
        private Size _screenSize;
        private Image _canvas;

        private ColorPalette _colorPalette;

        private ColorPalette Palette
        {
            get
            {
                return _colorPalette ?? InitPalette(PaletteParams.Reversed);
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitPalette(bool isReversed)
        {
            //TODO: REWRITE PALETTE INIT
            _colorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;
            if (isReversed)
            {
                for (int i = 0; i < 256; i++)
                {
                    var r = PaletteParams.R * i;
                    var g = PaletteParams.G * i;
                    var b = PaletteParams.B * i;
                    _colorPalette.Entries[255 - i] = Color.FromArgb(255, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    var r = PaletteParams.R * i;
                    var g = PaletteParams.G * i;
                    var b = PaletteParams.B * i;

                    _colorPalette.Entries[i] = Color.FromArgb(255, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                }
            }
            return _colorPalette;
        }

        private int TrimToByteRange(int value)
        {
            return value > 255 ? 255 : value;
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


        public Image Draw(TileCreator.Tile[] tiles, Point pointOfView)
        {
            return _iDrawer.DrawItems(_tDrawer.DrawImage(_canvas, tiles, pointOfView, _screenSize, Palette), pointOfView, _screenSize);
        }

        public Image Draw(Point pointOfView)
        {
            return _iDrawer.DrawItems(_canvas, pointOfView, _screenSize);
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
