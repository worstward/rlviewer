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

        /// <summary>
        /// Incapsulates radiolocation image file of a ".raw" format
        /// </summary>
        public Raw(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as RawHeader;
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

        private int _height;
        public override int Height
        {
            get
            {
                return _height = _height == 0 ? _header.ImgSize.Height : _height;
            }
            protected set
            {
                _height = value;
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

