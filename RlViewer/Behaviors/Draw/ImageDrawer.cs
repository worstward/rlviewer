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
        public ImageDrawer(Scaling.Scaler scaler)
        {
            _scaler = scaler;
        }


        private Scaling.Scaler _scaler;

        protected Scaling.Scaler Scaler
        {
            get { return _scaler; }
        }


        private ColorPalette _colorPalette;

        public virtual ColorPalette Palette
        {
            get
            {
                return _colorPalette = _colorPalette ?? InitPalette(PaletteParams.R, PaletteParams.G, PaletteParams.B, PaletteParams.Reversed);
            }
        }


        /// <summary>
        /// Initializes look-up palette for 8bpp image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitPalette(int rFactor, int gFactor, int bFactor, bool isReversed)
        {
            //TODO: REWRITE PALETTE INIT
            ColorPalette colorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;

            const int alpha = 255;

            for (int i = 0; i < 256; i++)
            {
                var r = rFactor * i;
                var g = gFactor * i;
                var b = bFactor * i;
                if (isReversed)
                {
                    if (PaletteParams.Logarithmic)
                    {
                        colorPalette.Entries[255 - i] = Color.FromArgb(alpha, GroupValues(r), GroupValues(g), GroupValues(b));
                    }
                    else
                    {
                        colorPalette.Entries[255 - i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                    }
                }
                else
                {
                    if (PaletteParams.Logarithmic)
                    {
                        colorPalette.Entries[i] = Color.FromArgb(alpha, GroupValues(r), GroupValues(g), GroupValues(b));
                    }
                    else
                    {
                        colorPalette.Entries[i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                    }
                }
            }


            return colorPalette;
        }



        private int GroupValues(int val)
        {
            return (int)(val / 16) * 16;
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
        /// <param name="logarithmic">Determines if palette uses logarithmic colors</param>
        public void GetPalette(int R, int G, int B, bool reversed, bool logarithmic)
        {
            _colorPalette = null;
            PaletteParams.R = R;
            PaletteParams.G = G;
            PaletteParams.B = B;
            PaletteParams.Reversed = reversed;
            PaletteParams.Logarithmic = logarithmic;
        }

        private static class PaletteParams
        {
            private static bool _logarithmic;

            public static bool Logarithmic
            {
                get
                {
                    return _logarithmic;
                }
                set
                {
                    _logarithmic = value; 
                }
            }


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
