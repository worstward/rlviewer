using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace RlViewer.Behaviors.Draw
{
    public static class DrawingHelper
    {
        /// <summary>
        /// Returns rectangular part of image
        /// </summary>
        /// <param name="bmp">Source image</param>
        /// <param name="x">Left top X coord of rectangle</param>
        /// <param name="y">Left top Y coord of rectangle</param>
        /// <param name="w">Width of rectangle</param>
        /// <param name="h">Height of rectangle</param>
        /// <returns></returns>
        public static Bitmap Crop(Bitmap bmp, int x, int y, int w, int h)
        {
            Rectangle rect = new Rectangle(x, y, w, h);
            return bmp.Clone(rect, bmp.PixelFormat);
        }


        /// <summary>
        /// Returns image of given size from provided Bitmap
        /// </summary>
        /// <param name="bmp">Bitmap to resize</param>
        /// <param name="newSize">Size to set</param>
        /// <param name="mode"></param>
        /// <returns>Resized bitmap</returns>
        public static Bitmap Resize(Bitmap bmp, Size newSize, InterpolationMode mode = InterpolationMode.NearestNeighbor)
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

        public static byte[] Crop(byte[] initialImage, int initialWidth, int x, int y, int width, int height)
        {
            byte[] cropped = new byte[width * height];
            var initialOffset = y * initialWidth + x;

            using (var ms = new MemoryStream(initialImage))
            {
                ms.Seek(initialOffset, SeekOrigin.Begin);

                for (int i = 0; i < height * width; i += width)
                {
                    ms.Read(cropped, i, width);
                    ms.Seek(initialWidth - width, SeekOrigin.Current);
                }

            }
            return cropped;
        }

        public static byte[] Resize(byte[] initialImage, int initialWidth, int initialHeight, float scaleFactor)
        {
            var imgWidth = (int)(initialWidth * scaleFactor);
            var imgHeight = (int)(initialHeight * scaleFactor);

            byte[] newImage = new byte[imgWidth * imgHeight];

            int targetIdx = 0;

            for (int i = 0; i < imgHeight; i++)
            {
                int iUnscaled = (int)(i / scaleFactor);
                for (int j = 0; j < imgWidth; j++)
                {
                    int jUnscaled = (int)(j / scaleFactor);
                    newImage[targetIdx++] = initialImage[iUnscaled * initialWidth + jUnscaled];
                }
            }

            return newImage;
        }



        /// <summary>
        /// Creates 8bpp image from raw byte array
        /// </summary>
        /// <param name="imgData">Raw image data</param>
        /// <param name="tileWidth">Image width</param>
        /// <param name="tileHeight">Image height</param>
        /// <returns>Grayscale image</returns>
        public static Bitmap GetBmp(byte[] imgData, int tileWidth, int tileHeight, ColorPalette palette)
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
