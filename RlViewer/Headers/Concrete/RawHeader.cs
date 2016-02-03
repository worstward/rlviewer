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
        public override byte[] Signature
        {
            get
            {
                return _signature;
            }
        }

        private byte[] _signature = new byte[1];



        public override byte[] FillHeader(string path)
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
