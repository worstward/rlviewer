using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Hierarchy;

namespace RlViewer.Factories
{
    public abstract class FileFactory
    {
        public abstract LoadedFile Create(FileProperties properties);

        public static FileFactory GetFactory(FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Brl4Factory();
                case FileType.rl4:
                    return new Rl4Factory();
                case FileType.k:
                    return new RhgKFactory();
                default:
                    throw new NotSupportedException();
            }
        }


    }
}
