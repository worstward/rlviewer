using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Files.Rli.Concrete
{
    class Rl8 : Rl4
    {
        public Rl8(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {
            _header = header as RlViewer.Headers.Concrete.Rl8.Rl8Header;
        }

        private RlViewer.Headers.Concrete.Rl8.Rl8Header _header;
    }
}
