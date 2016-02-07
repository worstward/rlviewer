using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;


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
        private byte[] _header;
        private string _path;
        private HeaderInfoOutput[] _headerInfo;

        private async Task<byte[]> FillHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];


            using (var fs = System.IO.File.OpenRead(path))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }
            return header;
        }

        public override async Task<HeaderInfoOutput[]> GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                _header = await FillHeaderAsync(_path);
                CheckInfo(_header);
                _headerInfo = ParseHeader(_header);
            }
            return _headerInfo;
        }

        private void CheckInfo(byte[] header)
        {
            for (int i = 0; i < _signature.Length; i++)
            {
                if (_header[i] != _signature[i])
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
