using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Headers.Concrete.Ba
{
    class BaHeader : RlViewer.Headers.Abstract.LocatorFileHeader
    {
        public BaHeader(string path)
        {
        }

        protected override byte[] Signature
        {
            get
            {
                throw new NotImplementedException("Ba signature");
            }
        }

        public override int FileHeaderLength
        {
            get
            {
                return 0;
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


        private int _bytesPerSample = 4;
        private int _strHeaderLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Headers.Concrete.Ba.BaStrHeader));
        private int _headerLength = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Headers.Concrete.K.KFileHeaderStruct));
        
        

        protected override HeaderInfoOutput[] GetHeaderInfo()
        {
            throw new NotImplementedException("Ba header info");
        }
    }
}
