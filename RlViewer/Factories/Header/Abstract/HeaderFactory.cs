using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Factories.Header.Concrete;

namespace RlViewer.Factories.Header.Abstract
{
    public abstract class HeaderFactory
    {
        public abstract RlViewer.Headers.Abstract.LocatorFileHeader Create(string path);

        public static HeaderFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case RlViewer.FileType.brl4:                 
                    return new Brl4HeaderFactory();
                case RlViewer.FileType.rl4:
                    return new Rl4HeaderFactory(); 
                case RlViewer.FileType.raw:
                    return new RawHeaderFactory(); 
                case RlViewer.FileType.k:
                    return new RhgKHeaderFactory(); 
                case RlViewer.FileType.r:
                    return new RHeaderFactory();
                default:
                    throw new NotSupportedException("Unsupported filter type");
            }
        }
    }
}
