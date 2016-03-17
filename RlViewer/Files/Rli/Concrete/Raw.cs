using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Raw;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Files.Rli.Concrete
{
    public class Raw : RliFile
    {
        public Raw(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as RawHeader;
            Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Raw file opened: {0}", properties.FilePath));
        }

        private RawHeader _header;

        public override LocatorFileHeader Header
        {
            get { return _header; }
        }

        public override Navigation.NavigationContainer Navigation
        {
            get
            {
                return null;
            }
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

