using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;


namespace RlViewer.Headers.Concrete.Raw
{
    class RawHeader : FileHeader
    {

        public RawHeader(string path)
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

        private int _headerLength = 0;
        private byte[] _signature = new byte[1];
        private byte[] _header;
        private string _path;
        private HeaderInfoOutput[] _headerInfo;
        
        public override HeaderInfoOutput[] GetHeaderInfo()
        {
            return new HeaderInfoOutput[] { new HeaderInfoOutput("Инфо", new List<Tuple<string, string>>()) };
        }

    }
}
