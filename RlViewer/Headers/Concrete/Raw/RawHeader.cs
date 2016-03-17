using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;


namespace RlViewer.Headers.Concrete.Raw
{
    class RawHeader : LocatorFileHeader
    {
        public RawHeader(string path)
        {
            _path = path;
            using (var sizeFrm = new Forms.SizeForm())
            {
                if (sizeFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _imgSize = sizeFrm.ImgSize;
                }

            }
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

        public override HeaderInfoOutput[] HeaderInfo
        {
            get
            {
                return _headerInfo = _headerInfo ?? GetHeaderInfo();
            }
        }

        private int _bytesPerSample = 4;
        private const int _strHeaderLength = 0;
        private const int _headerLength = 0;
        private byte[] _signature = new byte[_headerLength];
        private byte[] _header = new byte[_headerLength];
        private string _path;
        private HeaderInfoOutput[] _headerInfo;
        private System.Drawing.Size _imgSize = new System.Drawing.Size();

        public System.Drawing.Size ImgSize
        {
            get
            {
                return _imgSize;
            }
        }


        protected override HeaderInfoOutput[] GetHeaderInfo()
        {
            return new HeaderInfoOutput[] 
            { 
                new HeaderInfoOutput("Инфо", new List<Tuple<string, string>>() 
                {
                    new Tuple<string, string>("Ширина, пикс", _imgSize.Width.ToString()),
                    new Tuple<string, string>("Высота, пикс", _imgSize.Height.ToString()) 
                })
            };
        }

    }
}
