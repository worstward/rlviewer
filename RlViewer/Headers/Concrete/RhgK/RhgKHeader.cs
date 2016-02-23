using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Rl4;

namespace RlViewer.Headers.Concrete
{
    class RhgKHeader : FileHeader
    {
        public RhgKHeader(string path)
        {
            _path = path;
        }


        protected override byte[] Signature
        {
            get
            {
                return _signature;
            }
        }
  
        public override int HeaderLength
        {
            get { return _headerLength; }
        }

        private int _headerLength = 800;
        private byte[] _signature = new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFE, 0x01, 0xFC, 0x01, 0xF8, 0x01, 0xF0, 0x01, 0xAA, 0x55, 0xAA, 0x56 };
        private string _path;
        private HeaderInfoOutput[] _headerInfo;
        private Rl4RliFileHeader _headerStruct;

        private byte[] ReadHeader(string path)
        {
            byte[] header = new byte[HeaderLength];

            using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                fs.Read(header, 0, header.Length);
            }

            return header;
        }

        public override HeaderInfoOutput[] GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                byte[] _header = ReadHeader(_path);

                using (var ms = new System.IO.MemoryStream(_header))
                {
                    _headerStruct = LocatorFile.ReadStruct<Rl4RliFileHeader>(ms);
                }
                CheckInfo(_headerStruct.fileSign);
                //_headerInfo = ParseHeader(_headerStruct);
            }
            return _headerInfo;
        }

        private HeaderInfoOutput[] ParseHeader(byte[] header)
        {
            throw new NotImplementedException();
        }       
    }

}
