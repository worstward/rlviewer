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
        protected LocatorFile(FileProperties properties) : base(properties)
        {
        }

        public abstract FileHeader Header { get; }


    }
}
