using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Files;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Rl4;

namespace RlViewer.Headers.Concrete
{
    class RhgKHeader : LocatorFileHeader
    {
        public RhgKHeader(string path)
        {
            _headerStruct = ReadHeader<Rl4RliFileHeader>(path);
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
            get { throw new NotImplementedException(); }
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

        private int _bytesPerSample = 2;
        private int _headerLength = 800;
        private byte[] _signature = new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFE, 0x01, 0xFC, 0x01, 0xF8, 0x01, 0xF0, 0x01, 0xAA, 0x55, 0xAA, 0x56 };

        private HeaderInfoOutput[] _headerInfo;
        private Rl4RliFileHeader _headerStruct;


        protected override HeaderInfoOutput[] GetHeaderInfo()
        {
            HeaderInfoOutput[] parsedHeader = null;

            try
            {
                parsedHeader = null;//ParseHeader(headerStruct);
            }
            catch (ArgumentException)
            {
                return null;
            }

            return parsedHeader;
        }





        private HeaderInfoOutput[] ParseHeader(byte[] header)
        {
            throw new NotImplementedException();
        }       
    }

}
