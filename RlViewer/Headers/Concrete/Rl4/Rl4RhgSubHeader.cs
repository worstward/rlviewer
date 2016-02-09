﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace RlViewer.Headers.Concrete.Rl4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct RhgSubHeaderStruct
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] fileTime;

        // формат файла
        public long fileLength;
        public long fileHeaderLength;
        public long fileTailLength;

        // тип файла
        byte type; // 255

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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3754)]
        public byte[] reserved2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] fileName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] reserved3;
    }
}