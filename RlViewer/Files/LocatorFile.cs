using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;

namespace RlViewer.Files
{
    public abstract class LocatorFile : LoadedFile, IHeader
    {
        protected LocatorFile(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties)
        {

        }

        public abstract Navigation.NavigationContainer Navigation { get; }
        public abstract LocatorFileHeader Header { get; }

        public abstract int Width { get; }
        public abstract int Height { get; }


    }
}
