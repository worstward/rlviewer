using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Rl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rl4StrHeaderStruct : Abstract.IStrHeader
    {
        [MarshalAs(UnmanagedType.I1)]
	    public bool isNavigation;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] time;

        public double Vx;
        public double Vy;
        public double Vz;

        public double latitude;
        public double longtitude;

        public double H;
        public double V;

        public double Ve;
        public double Vn;

        public double a;

        public double g;
        public double f;
        public double w;

        public double Vu;

        public double WH;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 119)]
        public byte[] reserved;
    }

}
