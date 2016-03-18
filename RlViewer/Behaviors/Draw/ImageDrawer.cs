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


        private ColorPalette colorPalette;

        public virtual ColorPalette Palette
        {
            get
            {
                return colorPalette ?? InitPalette(PaletteParams.R, PaletteParams.G, PaletteParams.B, PaletteParams.Reversed);
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitPalette(int rFactor, int gFactor, int bFactor, bool isReversed)
        {
            //TODO: REWRITE PALETTE INIT
            colorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;

            const int alpha = 255;

            for (int i = 0; i < 256; i++)
            {
                var r = rFactor * i;
                var g = gFactor * i;
                var b = bFactor * i;
                if (isReversed)
                {
                    colorPalette.Entries[255 - i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                }
                else
                {
                    colorPalette.Entries[i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                }
            }


            return colorPalette;
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
            colorPalette = null;
            PaletteParams.R = R;
            PaletteParams.G = G;
            PaletteParams.B = B;
            PaletteParams.Reversed = reversed;
        }

        private static class PaletteParams
        {

            private static bool reversed;

            public static bool Reversed
            {
                get
                {
                    return reversed;
                }
                set
                {
                    reversed = value;
                }
            }



            private static int red = 1;
            public static int R
            {
                get
                {
                    return red;
                }
                set
                {
                    red = value < 1 ? 0 : value;
                }
            }

            private static int green = 1;
            public static int G
            {
                get
                {
                    return green;
                }
                set
                {
                    green = value < 1 ? 0 : value;
                }
            }

            private static int blue = 1;
            public static int B
            {
                get
                {
                    return blue;
                }
                set
                {
                    blue = value < 1 ? 0 : value;
                }
            }
        }
    }
}
