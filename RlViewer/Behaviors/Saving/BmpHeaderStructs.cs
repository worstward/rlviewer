using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Behaviors.Saving
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public BITMAPFILEHEADER(uint bmpSize, uint offset, ushort type = 0x4D42)
        {
            bfType = type;
            bfSize = bmpSize;
            bfReserved1 = 0;
            bfReserved2 = 0;
            bfOffBits = offset;
        }

        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public BITMAPINFOHEADER(int width, int height, uint sizeImg = 0, uint bSize = 40, uint clrUsed = 256,
            uint clrImportant = 0, BitmapCompressionMode mode = BitmapCompressionMode.BI_RGB, ushort bpp = 8, ushort planes = 1)
        {
            biSize = bSize;
            biWidth = width;
            biHeight = height;
            biPlanes = planes;
            biBitCount = bpp;
            biCompression = mode;
            biSizeImage = sizeImg;
            biXPelsPerMeter = 0;
            biYPelsPerMeter = 0;
            biClrUsed = clrUsed;
            biClrImportant = clrImportant;
        }

        uint biSize;
        int biWidth;
        int biHeight;
        ushort biPlanes;
        ushort biBitCount;
        BitmapCompressionMode biCompression;
        uint biSizeImage;
        int biXPelsPerMeter;
        int biYPelsPerMeter;
        uint biClrUsed;
        uint biClrImportant;

    }

    public enum BitmapCompressionMode : uint
    {
        BI_RGB = 0,
        BI_RLE8 = 1,
        BI_RLE4 = 2,
        BI_BITFIELDS = 3,
        BI_JPEG = 4,
        BI_PNG = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBQUAD
    {
        public RGBQUAD(byte red, byte green, byte blue, byte reserved = 0)
        {
            rgbRed = red;
            rgbGreen = green;
            rgbBlue = blue;
            rgbReserved = reserved;
        }
        byte rgbBlue;
        byte rgbGreen;
        byte rgbRed;
        byte rgbReserved;
    }


}
