using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;

namespace RlViewer.Headers.Concrete
{
    class Rl4Header : FileHeader
    {
        public override byte[] Signature
        {
            get
            {
                return _signature;
            }
        }

        private byte[] _signature = new byte[] { 0x00, 0xFF, 0x00, 0xFF };



        public override byte[] FillHeader(string path)
        {
            throw new NotImplementedException();
        }


        private int _headerLength = 16384;
        public override int HeaderLength
        {
            get { return _headerLength; }
        }


    }
}
