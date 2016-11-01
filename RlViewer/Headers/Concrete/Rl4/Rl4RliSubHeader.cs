using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete.Rl4
{

    public enum SampleType : byte
    {
        Float = 2,
        Complex = 3
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rl4RliSubHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] fileTime;

        // формат файла
        public long fileLength;
        public long fileHeaderLength;
        public long fileTailLength;


        // тип отсчета 
        public SampleType type;

        // формат строки
        public int strHeaderLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] dummy1;

        public int strSignalCount;

        // размер кадра
        public int cadrWidth;
        public int cadrHeight;

        // размер изображения
        public int width;
        public int height;
        public int frames;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] synthTime;

        // размер соответствующего фрагмента РГГ
        public int processi;
        public int processj;

        // параметры упаковки
        public float u0;
        public float u1;

        public int v0;
        public int v1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] reserved2;

        // проекция
        public byte rangeType;

        // шаг разложения
        public float dx;
        public float dy;

        // флип
        public byte flipType;
        //135
        // смещение фрагмента изображения
        public int sx;
        public int sy;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3688)]
        public byte[] reserved3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] fileName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] reserved4;

    }
}
