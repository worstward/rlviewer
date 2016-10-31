using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using RlViewer.Files.Rhg.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.K;
using RlViewer.Navigation.Concrete;


namespace RlViewer.Files.Rhg.Concrete
{

    /// <summary>
    /// Incapsulates radiohologram file of a ".ba" format
    /// </summary>
    class Ba : RhgFile
    {
        public Ba(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as Headers.Concrete.Ba.BaHeader;
            _navi = navi;
        }


        private Headers.Concrete.Ba.BaHeader _header;
        public override LocatorFileHeader Header
        {
            get 
            {
                return _header; 
            }
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
                return _height = _height == 0 ? (int)(new System.IO.FileInfo(Properties.FilePath).Length /
                    (Width * _header.BytesPerSample + _header.StrHeaderLength)) 
                    : _height;
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
                return (int)8064;
            }
        }


    }
}
