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

        public override int FileHeaderLength
        {
            get { return _headerLength; }
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
        private const int _strHeaderLength = 0;
        private const int _headerLength = 0;
        private byte[] _signature = new byte[_headerLength];
        private byte[] _header    = new byte[_headerLength];
        private string _path;

        private System.Drawing.Size _imgSize = new System.Drawing.Size();

        public System.Drawing.Size ImgSize
        {
            get
            {
                if (_imgSize.Width == 0 || _imgSize.Height == 0)
                {
                    using(var sizeFrm = new SizeForm())
                    {
                        if (sizeFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            _imgSize = sizeFrm.ImgSize;
                        }
                    }
                }
                return _imgSize;
            }
        }


        public override HeaderInfoOutput[] GetHeaderInfo()
        {
            return new HeaderInfoOutput[] { new HeaderInfoOutput("Инфо", new List<Tuple<string, string>>()) };
        }

    }
}
