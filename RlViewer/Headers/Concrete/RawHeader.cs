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

        private byte[] _signature = new byte[1];
        private byte[] _header;
        private string _path;

        public async Task<byte[]> FillHeaderAsync(string path)
        {
            byte[] header = new byte[HeaderLength];
            
            using (var fs = System.IO.File.OpenRead(path))
            {
                await fs.ReadAsync(header, 0, header.Length);
            }
            return header;
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
        public override async Task<List<Tuple<string, string>>> GetHeaderInfo()
        {
            if (_headerInfo == null)
            {
                _header = await FillHeaderAsync(_path);
                CheckInfo(_header);
                return ParseHeader(_header);
            }
            return _headerInfo;
        }

        List<Tuple<string, string>> _headerInfo;
        private List<Tuple<string, string>> ParseHeader(byte[] header)
        {
            throw new NotImplementedException();
        }



        private int _headerLength = 0;
        public override int HeaderLength
        {
            get { return _headerLength; }
        }
    }
}
