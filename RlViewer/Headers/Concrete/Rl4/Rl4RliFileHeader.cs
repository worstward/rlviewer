using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Rl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Rl4RliFileHeader
    {
        // signature
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] fileSign;

        // version
        public int fileVersion;

        // rhg subheader
        [MarshalAs(UnmanagedType.Struct)]
        public RhgSubHeaderStruct rhgParams;

        // rli subheader
        [MarshalAs(UnmanagedType.Struct)]
        public Rl4SubHeaderStruct rlParams;

        // synthesis subheader
        [MarshalAs(UnmanagedType.Struct)]
        public SynthesisSubHeaderStruct synthParams;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4088)]
        public byte[] reserved;
    }
}
