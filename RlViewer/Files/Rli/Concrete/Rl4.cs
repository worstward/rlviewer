using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Behaviors.Draw;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Files.Rli.Concrete
{
    public class Rl4 : RliFile
    {
        public Rl4(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as Rl4Header;
            _navi = navi;
            //_navi = new RlViewer.Navigation.Navigation(properties, _header.HeaderStruct.synthParams.D0, _header.HeaderStruct.synthParams.dD,
            //    _header.HeaderStruct.synthParams.board, Header.FileHeaderLength, Width * Header.BytesPerSample); 
            Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("Rl4 file opened: {0}", properties.FilePath));
        }


        private Rl4Header _header;
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


        public override int Height
        {
            get
            {
                return _header.HeaderStruct.rlParams.height;    
            }
        }

        public override int Width
        {
            get
            {
                return _header.HeaderStruct.rlParams.width;
            }
        }


    }
}
