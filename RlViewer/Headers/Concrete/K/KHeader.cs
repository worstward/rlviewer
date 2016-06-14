using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Headers.Concrete.K
{
    class KHeader : RlViewer.Headers.Abstract.LocatorFileHeader
    {
        public KHeader(string path)
        {
            _headerStruct =  ReadHeader<KFileHeaderStruct>(path);
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
        private int _strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(new Headers.Concrete.K.KStrHeaderStruct());
        private int _headerLength = System.Runtime.InteropServices.Marshal.SizeOf(new Headers.Concrete.K.KFileHeaderStruct());
        private byte[] _signature = new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFE, 0x01, 0xFC, 0x01, 0xF8, 0x01, 0xF0, 0x01, 0xAA, 0x55, 0xAA, 0x56 };
        private KFileHeaderStruct _headerStruct;
        private HeaderInfoOutput[] _headerInfo;

        public KFileHeaderStruct HeaderStruct
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


        private HeaderInfoOutput[] ParseHeader(KFileHeaderStruct headerStruct)
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
            synthHeader.Add(new Tuple<string, string>("Номер канала", headerStruct.locatorHeader.channelNumber.ToString()));
            synthHeader.Add(new Tuple<string, string>("Режим оператора", headerStruct.locatorHeader.operatorMode.ToString()));
            synthHeader.Add(new Tuple<string, string>("Версия", headerStruct.locatorHeader.version.ToString()));

            return new HeaderInfoOutput[]
            {
                new HeaderInfoOutput("Параметры локатора", synthHeader),
            };
        }

    }
}
