using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete.Brl4
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SynthesisSubHeaderStruct
    {
        // алгоритм синтеза
        public byte processAlgorithm; // 255

        // параметры привязки/носителя
        [MarshalAs(UnmanagedType.I1)]
        public bool isHeaders1;
        [MarshalAs(UnmanagedType.I1)]
        public bool isHeaders2;

        // дальность
        public float D0;
        public float dD;
        public byte board;

        public float V;
        public float H;

        public float g;
        public float f;
        public float w;

        public float a;
        public float latitude;
        public float longtitude;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] time;

        // параметры сигнала
        public float VH;
        public float lambda;
        public float Fn;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 842)]
        public byte[] reserved2;

        // диапазон синтеза по азимуту
        [MarshalAs(UnmanagedType.I1)]
        public bool isProcessAlli;
        public int i1;
        public int i2;

        // диапазон синтеза по дальности
        [MarshalAs(UnmanagedType.I1)]
        public bool isProcessAllj;
        public int j1;
        public int j2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] reserved3;

        // параметры упаковки
        public byte type; // 2 - float, 3 - {float, float}

        public float u0;
        public float u1;

        public int v0;
        public int v1;

        // комментарий
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] comments;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] reserved4;

        // размер кадра
        public int cadrWidth;
        public int cadrHeight;

        // дальность
        public byte rangeType;

        // ближний край полосы
        public byte flipType;

        //поляризация сигнала
        public byte polarization; //0 – ВВ, 1 – ГГ, 2 – ВГ, 3 – ГВ

        // Угол зондирования относительно вектора скорости
        public float angle_zond; // +/- 90 градусов


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 887)]
        public byte[] reserved6;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] rhgName;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1453)]
        public byte[] reserved5;
    }




}
