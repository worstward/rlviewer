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

        private const int _headerLength = 0;
        private byte[] _signature = new byte[_headerLength];
        private byte[] _header    = new byte[_headerLength];
        private string _path;
        private HeaderInfoOutput[] _headerInfo;

        private System.Drawing.Size _imgSize = new System.Drawing.Size();

        public System.Drawing.Size ImgSize
        {
            get
            {
                if (_imgSize.Width == 0 && _imgSize.Height == 0)
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

        protected override void CheckInfo(byte[] header)
        {
            base.CheckInfo(header);
        }


        public override HeaderInfoOutput[] GetHeaderInfo()
        {
            return new HeaderInfoOutput[] { new HeaderInfoOutput("Инфо", new List<Tuple<string, string>>()) };
        }

    }
}
