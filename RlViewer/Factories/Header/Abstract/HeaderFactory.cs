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

        public abstract RlViewer.Headers.Abstract.LocatorFileHeader Create(Headers.Abstract.IHeaderStruct headerStruct);

        public static HeaderFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:                 
                    return new Brl4HeaderFactory();
                case FileType.rl4:
                    return new Rl4HeaderFactory(); 
                case FileType.raw:
                    return new RawHeaderFactory(); 
                case FileType.k:
                    return new KHeaderFactory(); 
                case FileType.r:
                    return new RHeaderFactory();
                case FileType.rl8:
                    return new Rl8HeaderFactory();
                case FileType.ba:
                    return new BaHeaderFactory();
                default:
                    throw new NotSupportedException("Unsupported file type");
            }
        }
    }
}
