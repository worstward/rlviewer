using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Factories.Saver.Concrete;

namespace RlViewer.Factories.Saver.Abstract
{
    public abstract class SaverFactory
    {
        public abstract RlViewer.Behaviors.Saving.Abstract.Saver Create(Files.LoadedFile file);

        public static SaverFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Brl4SaverFactory();
                case FileType.rl4:
                    return new Rl4SaverFactory();
                case FileType.raw:
                    return new RawSaverFactory();
                case FileType.k:
                    return new RhgKSaverFactory();
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }
    }
}
