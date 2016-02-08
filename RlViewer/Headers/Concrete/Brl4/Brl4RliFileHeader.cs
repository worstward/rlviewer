using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Brl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Brl4RliFileHeader
    {
        // сигнатура
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] fileSign;

        // версия
        public int fileVersion;

        // подзаголовок РГГ
        [MarshalAs(UnmanagedType.Struct)]
        public RhgSubHeaderStruct rhgParams;

        // подзаголовок РЛИ
        [MarshalAs(UnmanagedType.Struct)]
        public Rl4SubHeaderStruct rlParams;

        // подзаголовок параметров синтеза
        [MarshalAs(UnmanagedType.Struct)]
        public SynthesisSubHeaderStruct synthParams;

        //
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4088)]
        public byte[] reserved;
    }
}
