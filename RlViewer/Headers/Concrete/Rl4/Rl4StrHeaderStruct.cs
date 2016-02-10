using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Rl4
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Rl4StrHeaderStruct
    {
	    bool isNavigation;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	    byte[] time;

        double Vx;
        double Vy;
        double Vz;

	    double latitude;
	    double longtitude;

	    double H;
	    double V;

	    double Ve;
	    double Vn;

	    double a;

	    double g;
	    double f;
	    double w;

	    double Vu;

	    double WH;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 119)]
        byte reserved;
    }

}
