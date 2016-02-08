using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Concrete
{
    class RhgKHeader : FileHeader
    {
        public RhgKHeader(string path)
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
            get { return _headerLength; }
        }

        private int _headerLength = 800;
        private byte[] _signature = new byte[] { 0xFF, 0x00, 0xFF, 0x00, 0xFE, 0x01, 0xFC, 0x01, 0xF8, 0x01, 0xF0, 0x01, 0xAA, 0x55, 0xAA, 0x56 };
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
                //_headerInfo = ParseHeader(_headerStruct);
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

        private HeaderInfoOutput[] ParseHeader(byte[] header)
        {
            throw new NotImplementedException();
        }


       
    }



}
