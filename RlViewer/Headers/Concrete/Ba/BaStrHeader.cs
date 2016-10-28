using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete.Ba
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BaStrHeader : Abstract.IStrHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] reserved1;

        public ushort header;
        public ushort hours;
        public ushort minutes;
        public float seconds;
        public float totalTime;
        public float heading;
        public float pitch;
        public float roll;
        public float latitude;
        public float longtitude;
        public float H;
        public float Ve;
        public float Vn;
        public float Vh;
        public short Ax;
        public short Ay;
        public short Az;
        public ushort Fizp;
        public float V;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] reserved2;
        public short T;
        public ushort state;
        public ushort frameCounter;
        public ushort crc;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 380)]
        public byte[] reserved3;
    }
}
