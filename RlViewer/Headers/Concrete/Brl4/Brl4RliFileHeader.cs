using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Brl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Brl4RliFileHeader : Headers.Abstract.IHeaderStruct
    {
        public Brl4RliFileHeader(byte[] fileSign, int fileVersion, Brl4RhgSubHeaderStruct rhgParams,
            Brl4RliSubHeaderStruct rlParams, Brl4SynthesisSubHeaderStruct synthParams, int aligningPointsCount, 
            float rangeCompressionCoef, float azimuthCompressionCoef, byte[] reserved)
        {
            this.fileSign = fileSign;
            this.fileVersion = fileVersion;
            this.rhgParams = rhgParams;
            this.rlParams = rlParams;
            this.synthParams = synthParams;
            this.reserved = reserved;
            this.aligningPointsCount = aligningPointsCount;
            this.rangeCompressionCoef = rangeCompressionCoef;
            this.azimuthCompressionCoef = azimuthCompressionCoef;
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

        public int aligningPointsCount;

        public float rangeCompressionCoef;

        public float azimuthCompressionCoef;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4076)]
        public byte[] reserved;

        
    }
}
