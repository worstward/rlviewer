using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete.Rl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rl4RliSubHeaderStruct
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] fileTime;

        // формат файла
        public long fileLength;
        public long fileHeaderLength;
        public long fileTailLength;

        // тип файла
        public byte type; // 2 - float, 3 - {float, float}

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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] dummy4;

        // время синтеза
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] processTime;

        // размер соответствующего фрагмента РГГ
        public int processi;
        public int processj;

        // параметры упаковки
        public float u0;
        public float u1;

        public int v0;
        public int v1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] reserved2;

        // проекция
        public byte rangeType;

        // шаг разложения
        public float dx;
        public float dy;

        // флип
        byte flipType;

        // смещение фрагмента изображения
        public int sx;
        public int sy;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3688)]
        public byte[] reserved3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] fileName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] reserved4;


        //public static explicit operator RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct(Rl4RliSubHeaderStruct rl4RliSubHeader)
        //{
        //    byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Rl4RliSubHeaderStruct>(rl4RliSubHeader);

        //    using (var ms = new System.IO.MemoryStream(headerStructArr))
        //    {
        //        return RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct>(ms);
        //    }
        //}

        
    }
}
