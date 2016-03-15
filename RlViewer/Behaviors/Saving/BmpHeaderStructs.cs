using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Behaviors.Saving
{
    //https://msdn.microsoft.com/en-us/library/windows/desktop/dd183391(v=vs.85).aspx
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        short bfType;
        int bfSize;
        short bfReserved1;
        short bfReserved2;
        int bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPCOREHEADER 
    {
        int bcSize;
        short  bcWidth;
        short bcHeight;
        short bcPlanes;
        short bcBitCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER 
    {
        int biSize;
        long  biWidth;
        long  biHeight;
        short  biPlanes;
        short biBitCount;
        int biCompression;
        int  biSizeImage;
        long  biXPelsPerMeter;
        long  biYPelsPerMeter;
        int biClrUsed;
        int biClrImportant;
    }



}
