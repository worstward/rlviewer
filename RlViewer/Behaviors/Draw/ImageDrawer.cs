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
        private ColorPalette InitPalette(float rFactor, float gFactor, float bFactor, bool isReversed)
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
                    if (PaletteParams.IsGroupped)
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
                    if (PaletteParams.IsGroupped)
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



        private int GroupValues(float val)
        {
            return ((int)val / 16) * 16;
        }


        private int TrimToByteRange(float value)
        {
            var bVal = (int)(value > 255 ? 255 : value);
            bVal = bVal < 0 ? 0 : bVal;
            return bVal;
        }

        /// <summary>
        /// Changes palette color table according to input RGB values
        /// </summary>
        /// <param name="R">Red channel</param>
        /// <param name="G">Green channel</param>
        /// <param name="B">Blue channel</param>
        /// <param name="reversed">Determines if colors in color table are reversed</param>
        /// <param name="logarithmic">Determines if palette uses grouped colors</param>
        public void GetPalette(float R, float G, float B, bool reversed, bool grouped)
        {
            _colorPalette = null;
            PaletteParams.R = R;
            PaletteParams.G = G;
            PaletteParams.B = B;
            PaletteParams.Reversed = reversed;
            PaletteParams.IsGroupped = grouped;
        }

        private static class PaletteParams
        {
            private static bool _isGroupped;

            public static bool IsGroupped
            {
                get
                {
                    return _isGroupped;
                }
                set
                {
                    _isGroupped = value; 
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



            private static float _red = 1;
            public static float R
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

            private static float _green = 1;
            public static float G
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

            private static float _blue = 1;
            public static float B
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
