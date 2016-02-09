using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;


namespace RlViewer.Headers.Concrete
{
    class RawHeader : FileHeader
    {

        public RawHeader(string path)
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

        private int _headerLength = 0;
        private byte[] _signature = new byte[1];
        private byte[] _header;
        private string _path;
        private HeaderInfoOutput[] _headerInfo;

        private byte[] FillHeader(string path)
        {
            byte[] header = new byte[HeaderLength];
            
            using (var fs = System.IO.File.OpenRead(path))
            {
                fs.Read(header, 0, header.Length);
            }
            return header;
        }

        public override HeaderInfoOutput[] GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                _header = FillHeader(_path);
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
