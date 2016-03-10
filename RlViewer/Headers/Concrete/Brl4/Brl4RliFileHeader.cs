using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Brl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Brl4RliFileHeader
    {
        public Brl4RliFileHeader(byte[] fileSign, int fileVersion, Brl4RhgSubHeaderStruct rhgParams,
            Brl4RliSubHeaderStruct rlParams, Brl4SynthesisSubHeaderStruct synthParams, byte[] reserved)
        {
            this.fileSign = fileSign;
            this.fileVersion = fileVersion;
            this.rhgParams = rhgParams;
            this.rlParams = rlParams;
            this.synthParams = synthParams;
            this.reserved = reserved;
        }


        // сигнатура
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] fileSign;

        // версия
        public int fileVersion;

        // подзаголовок РГГ
        [MarshalAs(UnmanagedType.Struct)]
        public Brl4RhgSubHeaderStruct rhgParams;

        // подзаголовок РЛИ
        [MarshalAs(UnmanagedType.Struct)]
        public Brl4RliSubHeaderStruct rlParams;

        // подзаголовок параметров синтеза
        [MarshalAs(UnmanagedType.Struct)]
        public Brl4SynthesisSubHeaderStruct synthParams;

        //
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4088)]
        public byte[] reserved;

       


    }
}
