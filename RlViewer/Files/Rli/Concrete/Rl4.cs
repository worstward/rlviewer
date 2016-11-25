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
        /// <summary>
        /// Incapsulates radiolocation image file of a ".rl4" format
        /// </summary>
        public Rl4(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as Rl4Header;
            _navi = navi;
        }


        private Rl4Header _header;
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


        private int _height;
        public override int Height
        {
            get
            {
                return _height = _height == 0 ? _header.HeaderStruct.rlParams.height : _height;
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
                return _header.HeaderStruct.rlParams.width;
            }
        }

        public override void SetHeight(int height)
        {
            base.SetHeight(height);

            var rlParams = _header.HeaderStruct.rlParams;
            rlParams.height = height;

            var headerStruct = _header.HeaderStruct;
            headerStruct.rlParams = rlParams;
            _header.HeaderStruct = headerStruct;

            Header.ChangeFileHeaderStruct<Headers.Concrete.Rl4.Rl4RliFileHeader>(headerStruct, Properties);
        }

    }
}
