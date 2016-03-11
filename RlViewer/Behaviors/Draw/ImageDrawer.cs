using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RlViewer.Behaviors.Draw
{
    public abstract class ImageDrawer
    {


        private ColorPalette _colorPalette;

        public virtual ColorPalette Palette
        {
            get
            {
                return _colorPalette ?? InitPalette(PaletteParams.R, PaletteParams.G, PaletteParams.B, PaletteParams.Reversed);
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitPalette(int rCoef, int gCoef, int bCoef, bool isReversed)
        {
            //TODO: REWRITE PALETTE INIT
            _colorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;

            const int alpha = 255;

            for (int i = 0; i < 256; i++)
            {
                var r = rCoef * i;
                var g = gCoef * i;
                var b = bCoef * i;
                if (isReversed)
                {
                    _colorPalette.Entries[255 - i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                }
                else
                {
                    _colorPalette.Entries[i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
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
