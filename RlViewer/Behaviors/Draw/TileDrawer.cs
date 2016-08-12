using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Concurrent;
using RlViewer.Behaviors.TileCreator;
using RlViewer.Files.Rli.Abstract;

namespace RlViewer.Behaviors.Draw
{

    /// <summary>
    /// Incapsulates tile output functions
    /// </summary>
    public class TileDrawer : ImageDrawer
    {
        public TileDrawer(RlViewer.Behaviors.Filters.Abstract.ImageFiltering filter, RlViewer.Behaviors.Scaling.Scaler scaler) : base(scaler)
        {
            _filter = filter;
        }

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;


        /// <summary>
        /// Creates image from visible parts of tiles
        /// </summary>
        /// <param name="canvas">Bitmap to draw on</param>
        /// <param name="screenSize">Size of output window (picturebox)</param>
        /// <param name="tiles">Array of Tile objects</param>
        /// <param name="leftTopPointOfView">Left-top corner coordinates of the visible image</param>
        /// <returns></returns>       
        public Image DrawImage(int width, int height, Tile[] tiles, Point leftTopPointOfView, Size screenSize, bool highRes)
        {
            IEnumerable<TileImageWrapper> wrappers;


            if (Scaler.ScaleFactor == 1)
            {
                wrappers = ScaleNormal(tiles, leftTopPointOfView, screenSize);
            }
            else if (Scaler.ScaleFactor > 1)
            {
                wrappers = ScaleUp(tiles, leftTopPointOfView, screenSize);
            }
            else
            {
                wrappers = ScaleDown(tiles, leftTopPointOfView, screenSize, highRes);
            }


            var palette = _filter.ApplyColorFilters(Palette);

            //var image = DrawWrappers(wrappers, screenSize);

            var image = GetBmp(wrappers, screenSize);

            image.Palette = palette;

            return image;
        }


        /// <summary>
        /// Draws imageWrapper with given image and its location
        /// </summary>
        /// <param name="tilesToDraw"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private Image DrawWrappers(IEnumerable<TileImageWrapper> tilesToDraw, Size screenSize)
        {
            Bitmap canvas = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format24bppRgb);

            using (var g = Graphics.FromImage(canvas))
            {
                foreach (var t in tilesToDraw)
                {
                    g.DrawImage(t.TileImage, t.Location);
                }
            }
            return canvas;
        }


        /// <summary>
        /// Returns image from visible tiles with scale factor < 1
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="leftTopPointOfView"></param>
        /// <param name="screenSize"></param>
        /// <param name="highRes">Determines if algorithm uses averaging to get downscaled image (true if it uses)</param>
        /// <returns></returns>
        private IEnumerable<TileImageWrapper> ScaleDown(Tile[] tiles, Point leftTopPointOfView, Size screenSize, bool highRes)
        {

            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY));

            int scale = (int)(1 / Scaler.ScaleFactor);
            int scalePower = (int)Math.Log(scale, 2);
            
            BlockingCollection<TileImageWrapper> tileImgWrappers = new BlockingCollection<TileImageWrapper>();

            //var tileImgWrappers = new List<TileImageWrapper>(visibleTiles.Count());

            Parallel.ForEach(visibleTiles, tile =>
            {
                byte[] imgData = tile.ReadData();
                byte[] sievedImage = new byte[imgData.Length >> scalePower >> scalePower];

                //scale by averaging nearby pixels
                int index = 0;

                for (int i = 0; i < tile.Size.Height * tile.Size.Width - tile.Size.Width; i += scale * tile.Size.Width)
                {
                    for (int j = i; j < i + tile.Size.Width; j += scale)
                    {
                        if (highRes)
                        {
                            int cumulative = 0;
                            for (int k = j; k < j + scale; k++)
                            {
                                cumulative += imgData[k];
                            }
                            sievedImage[index] = (byte)(cumulative >> scalePower);
                        }
                        else
                        {
                            sievedImage[index] = imgData[j];
                        }

                        index++;
                    }
                }

                var tw = new TileImageWrapper(sievedImage,
                    (int)((tile.LeftTopCoord.X - leftTopPointOfView.X) >> scalePower),
                    (int)((tile.LeftTopCoord.Y - leftTopPointOfView.Y) >> scalePower),
                    tile.Size.Width >> scalePower, tile.Size.Height >> scalePower);

                tileImgWrappers.Add(tw);
            });

            return tileImgWrappers;
        }

        /// <summary>
        /// Returns image from visible tiles with scale factor > 1
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="leftTopPointOfView"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private IEnumerable<TileImageWrapper> ScaleUp(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            int scaledScreenX = (int)Math.Ceiling(screenSize.Width / Scaler.ScaleFactor);
            int scaledScreenY = (int)Math.Ceiling(screenSize.Height / Scaler.ScaleFactor);

            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                scaledScreenX, scaledScreenY)).ToArray();

            BlockingCollection<TileImageWrapper> tileImgWrappers = new BlockingCollection<TileImageWrapper>();
            
            //g.ScaleTransform(1.0F, -1.0F);
            //g.TranslateTransform(0.0F, -(float)canvas.Height)
            Size cropS = new Size();
            Tile leftTopTile = default(Tile);

            foreach(var tile in visibleTiles)
            {
                //stores relative offset by X for visible part from the beginning of the current tile
                int shiftTileX = (int)(leftTopPointOfView.X - tile.LeftTopCoord.X);
                shiftTileX = shiftTileX < 0 ? 0 : shiftTileX;

                //stores relative offset by Y for visible part from the beginning of the current tile
                int shiftTileY = (int)(leftTopPointOfView.Y - tile.LeftTopCoord.Y);
                shiftTileY = shiftTileY < 0 ? 0 : shiftTileY;

                //if not all scaled tile is visible we only take the visible part.
                int croppedWidth = tile.Size.Width - shiftTileX >= scaledScreenX ? scaledScreenX : tile.Size.Width - shiftTileX;
                int croppedHeight = tile.Size.Height - shiftTileY >= scaledScreenY ? scaledScreenY : tile.Size.Height - shiftTileY;

                //determines resized canvas size
                Size resizedCanvasSize = new Size((int)(croppedWidth * Scaler.ScaleFactor), (int)(croppedHeight * Scaler.ScaleFactor));

                //take lefttop visible tile to measure offset for other tiles
                if (tile.Equals(visibleTiles.First()))
                {
                    leftTopTile = tile;
                    cropS = resizedCanvasSize;
                }

                //see pointToDraw description                    
                int x = tile.LeftTopCoord.X <= leftTopPointOfView.X ? 0 : cropS.Width +
                    ((tile.LeftTopCoord.X - leftTopTile.LeftTopCoord.X) / tile.Size.Width - 1) * tile.Size.Width * (int)Scaler.ScaleFactor;
                int y = tile.LeftTopCoord.Y <= leftTopPointOfView.Y ? 0 : cropS.Height +
                    ((tile.LeftTopCoord.Y - leftTopTile.LeftTopCoord.Y) / tile.Size.Height - 1) * tile.Size.Height * (int)Scaler.ScaleFactor;

                byte[] imgData = tile.ReadData();
                var img = GetBmp(imgData, tile.Size.Width, tile.Size.Height);
                var croppedImage = Crop(img, shiftTileX, shiftTileY, croppedWidth, croppedHeight);

                var resized = CopyToBpp(Resize(croppedImage, resizedCanvasSize), 8);

                tileImgWrappers.Add(new TileImageWrapper(resized, x, y));              
            }

            return tileImgWrappers;
        }

        /// <summary>
        /// Creates 8bpp image from raw byte array
        /// </summary>
        /// <param name="imgData">Raw image data</param>
        /// <param name="tileWidth">Image width</param>
        /// <param name="tileHeight">Image height</param>
        /// <returns>Grayscale image</returns>
        private Bitmap GetBmp(IEnumerable<TileImageWrapper> tilesToDraw, Size screenSize)
        {
            Bitmap bmp = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format8bppIndexed);

            
            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, new Size(bmp.Width + 400, bmp.Height + 400)),
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);

            foreach (var tile in tilesToDraw)
            { 
                IntPtr ptr = bmpData.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(tile.TileBytes, 0, ptr, tile.TileBytes.Length);
            }
            
            bmp.UnlockBits(bmpData);
            return bmp;
        }



        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hdcDst, int xDst, int yDst, int w, int h, IntPtr hdcSrc, int xSrc, int ySrc, int rop);
        static int SRCCOPY = 0x00CC0020;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO bmi, uint Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);
        static uint BI_RGB = 0;
        static uint DIB_RGB_COLORS = 0;
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public uint biSize;
            public int biWidth, biHeight;
            public short biPlanes, biBitCount;
            public uint biCompression, biSizeImage;
            public int biXPelsPerMeter, biYPelsPerMeter;
            public uint biClrUsed, biClrImportant;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 256)]
            public uint[] cols;
        }

        static uint MAKERGB(int r, int g, int b)
        {
            return ((uint)(b & 255)) | ((uint)((r & 255) << 8)) | ((uint)((g & 255) << 16));
        }

        /// <summary>
        /// Copies a bitmap into a 1bpp/8bpp bitmap of the same dimensions, fast
        /// </summary>
        /// <param name="b">original bitmap</param>
        /// <param name="bpp">1 or 8, target bpp</param>
        /// <returns>a 1bpp copy of the bitmap</returns>
        private System.Drawing.Bitmap CopyToBpp(System.Drawing.Bitmap b, int bpp)
        {
            if (bpp != 1 && bpp != 8) throw new System.ArgumentException("1 or 8", "bpp");

            // Plan: built into Windows GDI is the ability to convert
            // bitmaps from one format to another. Most of the time, this
            // job is actually done by the graphics hardware accelerator card
            // and so is extremely fast. The rest of the time, the job is done by
            // very fast native code.
            // We will call into this GDI functionality from C#. Our plan:
            // (1) Convert our Bitmap into a GDI hbitmap (ie. copy unmanaged->managed)
            // (2) Create a GDI monochrome hbitmap
            // (3) Use GDI "BitBlt" function to copy from hbitmap into monochrome (as above)
            // (4) Convert the monochrone hbitmap into a Bitmap (ie. copy unmanaged->managed)

            int w = b.Width, h = b.Height;
            IntPtr hbm = b.GetHbitmap(); // this is step (1)
            //
            // Step (2): create the monochrome bitmap.
            // "BITMAPINFO" is an interop-struct which we define below.
            // In GDI terms, it's a BITMAPHEADERINFO followed by an array of two RGBQUADs
            BITMAPINFO bmi = new BITMAPINFO();
            bmi.biSize = 40;  // the size of the BITMAPHEADERINFO struct
            bmi.biWidth = w;
            bmi.biHeight = h;
            bmi.biPlanes = 1; // "planes" are confusing. We always use just 1. Read MSDN for more info.
            bmi.biBitCount = (short)bpp; // ie. 1bpp or 8bpp
            bmi.biCompression = BI_RGB; // ie. the pixels in our RGBQUAD table are stored as RGBs, not palette indexes
            bmi.biSizeImage = (uint)(((w + 7) & 0xFFFFFFF8) * h / 8);
            bmi.biXPelsPerMeter = 1000000; // not really important
            bmi.biYPelsPerMeter = 1000000; // not really important
            // Now for the colour table.
            uint ncols = (uint)1 << bpp; // 2 colours for 1bpp; 256 colours for 8bpp
            bmi.biClrUsed = ncols;
            bmi.biClrImportant = ncols;
            bmi.cols = new uint[256]; // The structure always has fixed size 256, even if we end up using fewer colours
            if (bpp == 1) { bmi.cols[0] = MAKERGB(0, 0, 0); bmi.cols[1] = MAKERGB(255, 255, 255); }
            else { for (int i = 0; i < ncols; i++) bmi.cols[i] = MAKERGB(i, i, i); }
            // For 8bpp we've created an palette with just greyscale colours.
            // You can set up any palette you want here. Here are some possibilities:
            // greyscale: for (int i=0; i<256; i++) bmi.cols[i]=MAKERGB(i,i,i);
            // rainbow: bmi.biClrUsed=216; bmi.biClrImportant=216; int[] colv=new int[6]{0,51,102,153,204,255};
            //          for (int i=0; i<216; i++) bmi.cols[i]=MAKERGB(colv[i/36],colv[(i/6)%6],colv[i%6]);
            // optimal: a difficult topic: http://en.wikipedia.org/wiki/Color_quantization
            // 
            // Now create the indexed bitmap "hbm0"
            IntPtr bits0; // not used for our purposes. It returns a pointer to the raw bits that make up the bitmap.
            IntPtr hbm0 = CreateDIBSection(IntPtr.Zero, ref bmi, DIB_RGB_COLORS, out bits0, IntPtr.Zero, 0);
            //
            // Step (3): use GDI's BitBlt function to copy from original hbitmap into monocrhome bitmap
            // GDI programming is kind of confusing... nb. The GDI equivalent of "Graphics" is called a "DC".
            IntPtr sdc = GetDC(IntPtr.Zero);       // First we obtain the DC for the screen
            // Next, create a DC for the original hbitmap
            IntPtr hdc = CreateCompatibleDC(sdc); SelectObject(hdc, hbm);
            // and create a DC for the monochrome hbitmap
            IntPtr hdc0 = CreateCompatibleDC(sdc); SelectObject(hdc0, hbm0);
            // Now we can do the BitBlt:
            BitBlt(hdc0, 0, 0, w, h, hdc, 0, 0, SRCCOPY);
            // Step (4): convert this monochrome hbitmap back into a Bitmap:
            System.Drawing.Bitmap b0 = System.Drawing.Bitmap.FromHbitmap(hbm0);
            //
            // Finally some cleanup.
            DeleteDC(hdc);
            DeleteDC(hdc0);
            ReleaseDC(IntPtr.Zero, sdc);
            DeleteObject(hbm);
            DeleteObject(hbm0);
            //
            return b0;
        }

        private byte[] Crop(byte[] initialImage, int initialWidth, int x, int y, int width, int height)
        {
            byte[] cropped = new byte[width * height];
            var initialOffset = y * initialWidth + x;

            using (var ms = new MemoryStream(initialImage))
            {
                ms.Seek(initialOffset, SeekOrigin.Begin);

                for(int i = 0; i < height * width; i += width)
                {
                    ms.Read(cropped, i, width);
                    ms.Seek(initialWidth - width, SeekOrigin.Current);
                }
                
            }
            return cropped;
        }



        private byte[] Resize(byte[] initialImage, int imgWidth, int imgHeight, int initWidth, float scaleFactor)
        {
            byte[] newImage = new byte[(int)(initialImage.Length * scaleFactor * scaleFactor)];

            int targetIdx = 0;

            for (int i = 0; i < imgHeight; i++)
            {
                int iUnscaled = (int)(i / scaleFactor);
                for (int j = 0; j < imgWidth; j++)
                {
                    int jUnscaled = (int)(j / scaleFactor);
                    newImage[targetIdx] = initialImage[iUnscaled * initWidth + jUnscaled];
                    Interlocked.Increment(ref targetIdx);
                }
            }
 
            return newImage;
        }


        /// <summary>
        /// Returns image from visible tiles with 1:1 scale
        /// </summary>
        /// <param name="tiles"></param>
        /// <param name="leftTopPointOfView"></param>
        /// <param name="screenSize"></param>
        /// <returns></returns>
        private IEnumerable<TileImageWrapper> ScaleNormal(Tile[] tiles, Point leftTopPointOfView, Size screenSize)
        {
            var visibleTiles = tiles.Where(x => x.CheckVisibility(leftTopPointOfView,
                screenSize.Width, screenSize.Height));
            var tileImgWrappers = new List<TileImageWrapper>();

            foreach(var tile in visibleTiles)
            {
                var tileBytes = tile.ReadData();
                //Bitmap tileImg = GetBmp(tileBytes, tile.Size.Width, tile.Size.Height);
                int xToScreen = tile.LeftTopCoord.X - leftTopPointOfView.X;
                int yToScreen = tile.LeftTopCoord.Y - leftTopPointOfView.Y;
                var tw = new TileImageWrapper(tileBytes, xToScreen, yToScreen, tile.Size.Width, tile.Size.Height);
                tileImgWrappers.Add(tw);
            }

            return tileImgWrappers;
        }



        private static void printArray(byte[] arr, int w, int h)
        {
            for (int i = 0; i < h; ++i)
            {
                for (int j = 0; j < w; ++j)
                    Console.Write("{0} ", arr[i * w + j]);
                Console.WriteLine("");
            }
        }


        /// <summary>
        /// Returns rectangular part of image
        /// </summary>
        /// <param name="bmp">Source image</param>
        /// <param name="x">Left top X coord of rectangle</param>
        /// <param name="y">Left top Y coord of rectangle</param>
        /// <param name="w">Width of rectangle</param>
        /// <param name="h">Height of rectangle</param>
        /// <returns></returns>
        private Bitmap Crop(Bitmap bmp, int x, int y, int w, int h)
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
        private Bitmap Resize(Bitmap bmp, Size newSize, InterpolationMode mode = InterpolationMode.NearestNeighbor)
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

        /// <summary>
        /// Creates 8bpp image from raw byte array
        /// </summary>
        /// <param name="imgData">Raw image data</param>
        /// <param name="tileWidth">Image width</param>
        /// <param name="tileHeight">Image height</param>
        /// <returns>Grayscale image</returns>
        private Bitmap GetBmp(byte[] imgData, int tileWidth, int tileHeight)
        {
            Bitmap bmp = new Bitmap(tileWidth, tileHeight, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);
            
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(imgData, 0, ptr, imgData.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }


        private byte[] GetBytes(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);

            byte[] imgData = new byte[bmp.Width * bmp.Height];

            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptr, imgData, 0, imgData.Length);
            bmp.UnlockBits(bmpData);
            return imgData;
        }

    }
}
