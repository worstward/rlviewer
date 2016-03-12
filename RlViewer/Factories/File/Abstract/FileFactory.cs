using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Factories.File.Concrete;

namespace RlViewer.Factories.File.Abstract
{
    public abstract class FileFactory
    {

        public abstract LocatorFile Create(FileProperties properties);

        public static FileFactory GetFactory(FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Brl4Factory();
                case FileType.rl4:
                    return new Rl4Factory();
                case FileType.raw:
                    return new RawFactory();
                case FileType.k:
                    return new RhgKFactory();
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }

    }
}
