using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace RlViewer.Behaviors.Draw
{

    /// <summary>
    /// Base class for drawing locator image
    /// </summary>
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


        //private ColorPalette _colorPalette;

        public virtual ColorPalette Palette
        {
            get
            {
                return InitPalette(PaletteParams.R, PaletteParams.G, PaletteParams.B,
                    PaletteParams.Reversed, PaletteParams.UseTemperaturePalette);
            }
            
        }



        /// <summary>
        /// Initializes look-up palette for 8bpp image
        /// </summary>
        /// <returns>Color palette</returns>
        private ColorPalette InitPalette(float rFactor, float gFactor, float bFactor, bool isReversed, bool useTemperaturePalette)
        {
            //TODO: REWRITE PALETTE INIT
            ColorPalette colorPalette = new Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette;


            object obj = new object();
             

            if (useTemperaturePalette)
            {
                for (int i = 0; i < 256; i++)
                {
                    if (isReversed)
                    {
                        colorPalette.Entries[255 - i] = ColorFromHSV(255 - i, 1, 0.7f);
                    }
                    else
                    {
                        colorPalette.Entries[i] = ColorFromHSV(255 - i, 1, 0.7f);
                    }
                }

                return colorPalette;
            }



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
                        colorPalette.Entries[255 - i] = Color.FromArgb(alpha, TrimToByteRange(GroupValues(r)), TrimToByteRange(GroupValues(g)), TrimToByteRange(GroupValues(b)));
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
                        colorPalette.Entries[i] = Color.FromArgb(alpha, TrimToByteRange(GroupValues(r)), TrimToByteRange(GroupValues(g)), TrimToByteRange(GroupValues(b)));
                    }
                    else
                    {
                        //var color = Color.FromArgb(alpha, GroupValues(r), GroupValues(g), GroupValues(b));
                        colorPalette.Entries[i] = Color.FromArgb(alpha, TrimToByteRange(r), TrimToByteRange(g), TrimToByteRange(b));
                    }
                }
            }



            return colorPalette;
        }

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
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
        public void GetPalette(float R, float G, float B, bool reversed, bool grouped, bool useTemperaturePalette)
        {
            //_colorPalette = null;
            PaletteParams.R = R;
            PaletteParams.G = G;
            PaletteParams.B = B;
            PaletteParams.Reversed = reversed;
            PaletteParams.IsGroupped = grouped;
            PaletteParams.UseTemperaturePalette = useTemperaturePalette;
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

            private static bool _useTemperaturePalette = false;

            public static bool UseTemperaturePalette
            {
                get 
                { 
                    return PaletteParams._useTemperaturePalette;
                }
                set 
                { 
                    PaletteParams._useTemperaturePalette = value;
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
