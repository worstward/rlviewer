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

        //Speed (x-y-z axis respectively)
        public double Vx;
        public double Vy;
        public double Vz;

        public double latitude;
        public double longtitude;

        public double H;//Height

        public double V; //Speed

        //Speed (e-n axis)
        public double Ve;
        public double Vn;


        public double a;//Heading

        public double g;//Roll
        public double f;//Pitch

        public double w;//Drift

        //Speed(u axis)
        public double Vu;

        public double WH;//Speed/Height

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 119)]
        public byte[] reserved;
    }

}
