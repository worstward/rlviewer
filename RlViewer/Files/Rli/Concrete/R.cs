using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.R;
using RlViewer.Navigation.Concrete;


namespace RlViewer.Files.Rli.Concrete
{

    /// <summary>
    /// Incapsulates radiolocation image file of a ".r" format
    /// </summary>
    class R : Rli.Abstract.RliFile
    {
        public R(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as RHeader;
            _navi = navi;
        }


        private RHeader _header;
        public override LocatorFileHeader Header
        {
            get { return _header; }
        }


        private Navigation.NavigationContainer _navi;
        public override Navigation.NavigationContainer Navigation
        {
            get
            {
                return _navi;
            }
        }


        private int _height = 0;
        public override int Height
        {
            get
            {
                //(file size - file header size) / (string header size + string data size) = string count
                return _height == 0 ? _height = (int)((new System.IO.FileInfo(Properties.FilePath).Length
                    - Marshal.SizeOf(new Headers.Concrete.R.RFileHeaderStruct())) 
                    / (_header.HeaderStruct.lineInfoHeader.lineLength * _header.BytesPerSample + 
                       Marshal.SizeOf(new RlViewer.Headers.Concrete.R.RStrHeaderStruct())))
                    : _height;
            }
        }

        public override int Width
        {
            get
            {
                return (int)_header.HeaderStruct.lineInfoHeader.lineLength;
            }
        }


    }
}
