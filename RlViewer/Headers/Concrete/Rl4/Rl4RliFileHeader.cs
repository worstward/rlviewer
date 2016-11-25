using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Rl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rl4RliFileHeader : Headers.Abstract.IHeaderStruct
    {
        public Rl4RliFileHeader(byte[] fileSign, int fileVersion, Rl4RhgSubHeaderStruct rhgParams, 
            Rl4RliSubHeaderStruct rlParams, Rl4SynthesisSubHeaderStruct synthParams, byte[] reserved)
        {
            this.fileSign = fileSign;
            this.fileVersion = fileVersion;
            this.rhgParams = rhgParams;
            this.rlParams = rlParams;
            this.synthParams = synthParams;
            this.reserved = reserved;
        }

        // signature
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] fileSign;

        // version
        public int fileVersion;

        // rhg subheader
        [MarshalAs(UnmanagedType.Struct)]
        public Rl4RhgSubHeaderStruct rhgParams;

        // rli subheader
        [MarshalAs(UnmanagedType.Struct)]
        public Rl4RliSubHeaderStruct rlParams;

        // synthesis subheader
        [MarshalAs(UnmanagedType.Struct)]
        public Rl4SynthesisSubHeaderStruct synthParams;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4088)]
        public byte[] reserved;


        
    }
}
