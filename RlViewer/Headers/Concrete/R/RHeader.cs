using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Files;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Behaviors.Converters;



namespace RlViewer.Headers.Concrete.R
{
    class RHeader : RlViewer.Headers.Abstract.LocatorFileHeader
    {
        public RHeader(string path)
        {
            _headerStruct =  ReadHeader<RHeaderStruct>(path);
        }

        protected override byte[] Signature
        {
            get 
            {
                return _signature;
            }
        }

        public override int FileHeaderLength
        {
            get
            {
                return _headerLength;
            }
        }

        public override int StrHeaderLength
        {
            get 
            {
                return _strHeaderLength;
            }
        }
        public override int BytesPerSample
        {
            get
            {
                return _bytesPerSample;
            }
        }

        public override HeaderInfoOutput[] HeaderInfo
        {
            get
            {
                return _headerInfo = _headerInfo ?? GetHeaderInfo();
            }
        }



        private int _bytesPerSample = 4;
        private int _strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(new RHeaderStruct());
        private const int _headerLength = 16384;
        private byte[] _signature = new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFE, 0x01, 0xFC, 0x01, 0xF8, 0x01, 0xF0, 0x01, 0xAA, 0x55, 0xAA, 0x56 };
        private RHeaderStruct _headerStruct;
        private HeaderInfoOutput[] _headerInfo;


        public RHeaderStruct HeaderStruct
        {
            get
            {
                return _headerStruct;
            }
        }


        protected override HeaderInfoOutput[] GetHeaderInfo()
        {
            HeaderInfoOutput[] parsedHeader = null;

            try
            {
                parsedHeader = ParseHeader(_headerStruct);
            }
            catch (ArgumentException)
            {
                return null;
            }

            return parsedHeader;
        }


        private HeaderInfoOutput[] ParseHeader(RHeaderStruct headerStruct)
        {
            try
            {
                CheckInfo(headerStruct.signatureHeader.fileSign);
            }
            catch (ArgumentException aex)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Blocking, aex.Message);
                throw;
            }

            var rhgHeader = new List<Tuple<string, string>>();

            
          
            var synthHeader = new List<Tuple<string, string>>();
            synthHeader.Add(new Tuple<string, string>("Алгоритм синтеза",                headerStruct.synthesisHeader.processAlgorithm == 1 ? "ЕОК" : "Не определено"));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по дальности, м", headerStruct.synthesisHeader.dx.ToString()));
            synthHeader.Add(new Tuple<string, string>("Шаг разложения по азимуту, м", headerStruct.synthesisHeader.dy.ToString()));
            synthHeader.Add(new Tuple<string, string>("Начальная дальность, м",          headerStruct.synthesisHeader.initialRange.ToString()));
            synthHeader.Add(new Tuple<string, string>("Борт",                            headerStruct.synthesisHeader.sideObservation == 0 ? "Левый" : "Правый"));

            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("Синтез", synthHeader),
            };
        }
    }
}
