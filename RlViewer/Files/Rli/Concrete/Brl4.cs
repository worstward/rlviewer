using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Brl4;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Files.Rli.Concrete
{

    /// <summary>
    /// Incapsulates radiolocation image file of a ".brl4" format
    /// </summary>
    public class Brl4 : RliFile
    {
        public Brl4(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as Brl4Header;
            _navi = navi;
        }

        private Navigation.NavigationContainer _navi;
        public override Navigation.NavigationContainer Navigation
        {
            get
            {
                return _navi;
            }
        }

        private Brl4Header _header;

        public override LocatorFileHeader Header
        {
            get { return _header; }
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
