using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete.R
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RFileHeaderStruct
    {

	    public SignatureHeader		signatureHeader;		// сигнатура заголовка
        public FlightHeader flightHeader;			// параметры полета
        public LocatorHeader locatorHeader;			// режим локатора
        public ReceiverHeader receiverHeader;			// параметры приемника
        public TransmitterHeader transmitterHeader;		// параметры передатчика
        public SynchronizerHeader synchronizerHeader;		// параметры синхронизатора
        public GeneratorHeader generatorHeader;		// параметры генератора
        public SgoHeader sgoHeader;				// параметры СЖО
        public AntennaSystemHeader antennaSystemHeader;	// параметры антенной системы
        public AdcHeader adcHeader;				// параметры АЦП
        public SynthesisHeader synthesisHeader;		// параметры синтеза;
        public LineInfoHeader lineInfoHeader;			// формат строки
    };

    #region FileHeaderInnerStructs
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SignatureHeader
    {
        enum HeaderType : byte
        {
            Undefined = 0,
            RggFileHeader = 1,		//< заголовок файла РГГ
            RliFileHeader = 2,		//< заголовок файла РЛИ
            LineHeader = 3			//< заголовок строки
        };

        // signature
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] fileSign;


        HeaderType headerType;					// тип заголовка
        public byte version;					// номер версии сигнатур заголовка

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlightHeader
    {
        public byte version;							//< номер версии 
        long timeARM;							//< Время АРМ в  милисекундах с 1970
        long timeUTC;							//< Время UTC в  милисекундах с 1970
        public uint numberMission;						//< номер миссии
        public uint numberFlight;						//< номер полета 
        public uint numberPeriod;						//< номер периода наблюдения 
        public uint numberFile;							//< номер файла

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] country;				//< Cтрока в CP-1251 c нулем на конце

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] terrain;				//< Cтрока в CP-1251 c нулем на конце

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LocatorHeader
    {
        public byte version;		// номер версии 
        public byte locatorType;	// номер диапазона работы локатора
        public byte channelNumber;	// номер канала
        public byte operatorMode;	// режим работы оператора

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ReceiverHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TransmitterHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchronizerHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GeneratorHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SgoHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AntennaSystemHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AdcHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynthesisHeader
    {
        public byte version;	// номер версии 
        public float freqGen;	// Несущая частота	[МГц]
        public float freqDeviation;	// Девиация частоты [МГц]	
        public float widthImpulse;	// Длит импульса [мкс]
        public byte sideObservation;	// Борт наблюдения	
        public float initialRange;	// Начальная дальность	
        public float dx;	// Шаг разложения по дальности	
        public float dy;	// Шаг разложения по азимуту	
        public byte rangeChirpSign;	// Знак ЛЧМ по дальности	
        public byte azimuthChirpSign;	// Знак ЛЧМ по азимуту	
        public uint lineLength;	// Размер строки РГГ	
        public float angle;	// Угол раскрыва антенны	
        public byte processAlgorithm;	// Алгоритм синтеза 1 - EOK
        public uint NyStart;	// Начальная строка по азимуту	
        public uint NxStart;	// Начальный отсчет по дальности 
        public uint MLength;	// Размер блока по азимуту	
        public uint NLength;	// Размер блока по дальности	
        public uint azimuthAspectRatio;	// Коэффициент сжатия блока по азимуту	
        public uint rangeAspectRatio;	// Коэффициент сжатия блока по дальности	
        public uint MShift;	// Размер кадра по азимуту	
        public uint NShift;	// Размер кадра по дальности	
        public float MScale;	// Коэффициент сжатия кадра по азимуту	
        public float NScale;	// Коэффициент сжатия кадра по дальности	
        public byte doRangeCompress;	// Сжатие ЛЧМ по дальности	
        public float times_mc;	// Увеличение матрицы синтеза по азимуту	
        public float radioSuppression;	// Коэффициент подавления радиопомехи	
        public byte interpolate;	// Интерполяция	
        public uint PNStripes;	// Число полос по дальности	
        public uint PNShift;	// Смещение полосы по дальности	
        public uint PNLength;	// Длина полосы по дальности	
        public byte cutDoppler;	// Фильтр верхних частот по Допплеру	
        public uint dopplerMax;// Макс допплеровская частота	
        public uint dopplerMin;// Мин допплеровская частота	
        public byte EOK_With_CZT;	// Алгоритм Chirp-Z-Transform	
        public uint NLobs;// Число боковых лепестков sinc	
        public byte loopBlocking;	// Модифицированная интерполяция Столта	

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 398)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LineInfoHeader
    {
        enum NumberType : byte
        {
            Complex = 0,	//< комплексные
            Real = 1		//< действительные
        };
        enum DataType : byte
        {
            int8 = 0,		//< char
            int16 = 1,		//< short
            uint8 = 2,		//< uchar
            uint16 = 3,		//< ushort
            f32 = 4,		//< public float
            f64 = 5			//< double
        };
        enum DataOrder : byte
        { // определен только для комплексных данных
            Interleaved = 0,		//< re, im, re, im, re, im, …,
            Sequential = 1			//< re, re, re, …, im, im, im, …
        };

        NumberType numberType;		// тип данных
        DataType dataType;		// тип отсчета
        DataOrder dataOrder;		// Порядок данных
        public uint lineLength;		// Отсчетов в строке

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 57)]
        public byte[] reserved;
    };
    #endregion

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RStrHeaderStruct : Abstract.IStrHeader
    {
        public SignatureHeader signatureHeader; 				// сигнатура заголовка
        public NavigationHeader navigationHeader;				// параметры навигации
        public ControlReceiverHeader controlReceiverHeader;			// контрольные параметры приемника
        public ControlTransmitterHeader controlTransmitterHeader;		// контрольные параметры передатчика
        public ControlSynchronizerHeader controlSynchronizerHeader;		// контрольные параметры синхронизатора
        public ControlGeneratorHeader controlGeneratorHeader;			// контрольные параметры генератора
        public ControlSgoHeader controlSgoHeader;				// контрольные параметры СЖО
        public ControlAntennaSystemHeader controlAntennaSystemHeader;		// контрольные параметры антенной системы
        public ControlAdcHeader controlAdcHeader;				// контрольные параметры АЦП
    };


    #region RliLineInnerStructs

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NavigationHeader
    {
        public byte version;//Версия заголовка
        public long timeArm;//Время АРМ
        public long timeGps;//Время GPS
        public uint lineNumber;//Номер строки

        [MarshalAs(UnmanagedType.I1)]
        public bool snp; //Данные СНП

        public double heightSk42;//Высота (СК-42)
        public double kursAngle; //Путевой угол
        public double latitudeSk42;//Широта (СК-42)
        public double latitudeExact;//Широта (точно)
        public double longtitudeSk42;//Долгота (СК-42)	
        public double longtitudeExact;//Долгота (точно)	
        public uint outputDelay;//Задержка выдачи
        public double verticalSpeed;//Вертикальная скорость


        //Составл пут скор, Wn
        //Составл пут скор, We
        //СКО горизонт корд
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] reserved1;

         [MarshalAs(UnmanagedType.I1)]
        public bool ins; //Данные ИНС

        public double latitudeIns; //широта ИНС
        public double longtitudeIns;//долгота ИНС
        public double speedLatIns;//скорость по широте ИНС
        public double speedLongIns;//скорость по долготе ИНС
        public double speedIns;//путевая скорость
        public double realTraceIns;//истинный курс
        public double realAngleIns;//истинный путевой угол
        public double hiroscopeTraceIns;//гироскопический курс
        public double driftAngleIns;//угол сноса
        public double tiltAngleIns;//угол крена
        public double tangageAngleIns;//угол тангажа

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        public byte[] reserved2;

         [MarshalAs(UnmanagedType.I1)]
        public bool insSns; //Данные ИНС/СНС
        public double latitudeInsSns; //широта
        public double longtitudeInsSns;//долгота
        public double heightInsSns;
        public double speedLatInsSns;
        public double speedLongInsSns;
        public double realTraceInsSns;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 428)]
        public byte[] reserved3;


    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlReceiverHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlTransmitterHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlSynchronizerHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlGeneratorHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlSgoHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlAntennaSystemHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ControlAdcHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] reserved;
    };


#endregion

}
