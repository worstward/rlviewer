using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete
{
    class Rl4Header : FileHeader
    {
        public Rl4Header(string path)
        {
            _path = path;
        }

        public override byte[] Signature
        {
            get 
            {
                return _signature;
            }
        }
  
        public override int HeaderLength
        {
            get
            {
                return _headerLength;
            }
        }

        private int _headerLength = 16384;
        private byte[] _signature = new byte[] { 0x52, 0x4c, 0x49, 0x00 };
        
        private string _path;
        private HeaderInfoOutput[] _headerInfo;
        private RliFileHeader _headerStruct;

        private async Task<byte[]> ReadHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];

            using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }
       
            return header;
        }

        public override async Task<HeaderInfoOutput[]> GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                byte[] _header = await ReadHeaderAsync(_path);

                using (var ms = new System.IO.MemoryStream(_header))
                {
                    _headerStruct = await ReadStruct<RliFileHeader>(ms);                  
                }
                CheckInfo(_headerStruct);
                _headerInfo = ParseHeader(_headerStruct);
            }
            return _headerInfo;
        }


        private void CheckInfo(RliFileHeader headerStruct)
        {
            for (int i = 0; i < headerStruct.fileSign.Length; i++)
            {
                if (headerStruct.fileSign[i] != _signature[i])
                {
                    throw new ArgumentException("Header signature");
                }
            }
        }



        private HeaderInfoOutput[] ParseHeader(RliFileHeader headerStruct)
        {
            var rhgHeader = new List<Tuple<string, string>>();
            rhgHeader.Add(new Tuple<string, string>("Имя файла", System.IO.Path.GetFileName(Encoding.UTF8.GetString(headerStruct.rhgParams.fileName).Trim('\0'))));
            rhgHeader.Add(new Tuple<string, string>("Размер файла",                      headerStruct.rhgParams.fileLength.ToReadableFileSize()));
            rhgHeader.Add(new Tuple<string, string>("Дата и время создания",             headerStruct.rhgParams.fileTime.ToDateTime().ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по дальности",     headerStruct.rhgParams.cadrWidth.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в кадре по азимуту",       headerStruct.rhgParams.cadrHeight.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в РГГ по дальности",       headerStruct.rhgParams.width.ToString()));
            rhgHeader.Add(new Tuple<string, string>("Отсчетов в РГГ по азимуту",         headerStruct.rhgParams.height.ToString()));
        
            var rliHeader = new List<Tuple<string, string>>();
            rliHeader.Add(new Tuple<string, string>("Размер файла",                      headerStruct.rlParams.fileLength.ToReadableFileSize()));
            rliHeader.Add(new Tuple<string, string>("Дата и время создания",             headerStruct.rlParams.fileTime.ToDateTime().ToString()));
            rliHeader.Add(new Tuple<string, string>("Ширина, отсчетов",                  headerStruct.rlParams.width.ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота, строк",                     headerStruct.rlParams.height.ToString())); 
            rliHeader.Add(new Tuple<string, string>("Ширина кадра, отсчетов",            headerStruct.rlParams.cadrWidth.ToString()));
            rliHeader.Add(new Tuple<string, string>("Высота кадра, строк",               headerStruct.rlParams.cadrHeight.ToString()));
            rliHeader.Add(new Tuple<string, string>("Кадров",                           (headerStruct.rlParams.height / headerStruct.rlParams.cadrHeight).ToString()));
            rliHeader.Add(new Tuple<string, string>("Тип файла/упаковка",                headerStruct.rlParams.type.ToRliFileType()));
            rliHeader.Add(new Tuple<string, string>("Тип дальности",                     headerStruct.rlParams.rangeType == 0 ? "Наклонная" : "Не определено"));
            rliHeader.Add(new Tuple<string, string>("Шаг разложения по дальности, м",    headerStruct.rlParams.dx.ToString()));
            rliHeader.Add(new Tuple<string, string>("Шаг разложения по азимуту, м",      headerStruct.rlParams.dy.ToString()));

            var synthHeader = new List<Tuple<string, string>>();
            synthHeader.Add(new Tuple<string, string>("Частота повторения, Гц", headerStruct.synthParams.Fn.ToString()));           
            synthHeader.Add(new Tuple<string, string>("Начальная дальность, м", headerStruct.synthParams.D0.ToString()));
            synthHeader.Add(new Tuple<string, string>("Скорость, м/с", headerStruct.synthParams.VH.ToString()));
            synthHeader.Add(new Tuple<string, string>("Борт", headerStruct.synthParams.board == 0 ? "Левый" : "Правый"));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по наклонной дальности, м", headerStruct.synthParams.dD.ToString()));
            synthHeader.Add(new Tuple<string, string>("Длина волны", headerStruct.synthParams.lambda.ToString()));

            var comments = new List<Tuple<string, string>>();
            comments.Add(new Tuple<string, string>("Комментарии", System.IO.Path.GetFileName(Encoding.UTF8.GetString(headerStruct.synthParams.comments).Trim('\0'))));


            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("РГГ", rhgHeader),
                new HeaderInfoOutput("РЛИ", rliHeader),
                new HeaderInfoOutput("Синтез", synthHeader),
                new HeaderInfoOutput("Комментарии", comments)
            };
        }


    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct RliFileHeader
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


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Rl4SubHeaderStruct
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
    }

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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] reserved1;

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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2596)]
        public byte[] reserved5;
    }



    




}
