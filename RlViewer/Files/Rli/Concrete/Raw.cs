using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Raw;


namespace RlViewer.Files.Rli.Concrete
{
    public class Raw : RliFile
    {
        public Raw(FileProperties properties) : base(properties)
        {
            _header = new RawHeader(properties.FilePath);
            Logging.Logger.Log(Logging.SeverityGrades.Info, "Raw file opened");
        }

        private RawHeader _header;

        public override FileHeader Header
        {
            get { return _header; }
        }

        public override int Height
        {
            get
            { 
                return _header.ImgSize.Height;
            }
        }
        public override int Width
        {
            get 
            {
                return _header.ImgSize.Width;
            }
        }

    }
}

