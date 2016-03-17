using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rhg.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Files.Rhg.Concrete
{
    public class RhgK : RhgFile
    {
        public RhgK(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as RhgKHeader;
        }


        private LocatorFileHeader _header;
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

        public override int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }


    }
}
