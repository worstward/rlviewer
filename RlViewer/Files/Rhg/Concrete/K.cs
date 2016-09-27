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
    /// Incapsulates radiohologram file of a ".k" format
    /// </summary>
    public class K : RhgFile
    {
        public K(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as KHeader;
            _navi = navi;
        }


        private KHeader _header;
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
                    - Marshal.SizeOf(typeof(Headers.Concrete.K.KFileHeaderStruct)))
                    / (_header.HeaderStruct.lineInfoHeader.lineLength * _header.BytesPerSample +
                       Marshal.SizeOf(typeof(RlViewer.Headers.Concrete.K.KStrHeaderStruct)))) : _height;
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
                return (int)_header.HeaderStruct.lineInfoHeader.lineLength;
            }
        }


    }
}
