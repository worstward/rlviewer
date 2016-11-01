using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.ImageMirrorer
{
    class ImageMirrorerFactory
    {
        public static Behaviors.ImageMirroring.Abstract.ImageMirrorer Create(Files.LocatorFile sourceFile)
        {
            switch (sourceFile.Properties.Type)
            {
                case FileType.brl4:
                    return new Behaviors.ImageMirroring.Concrete.Brl4ImageMirrorer(sourceFile);
                case FileType.rl4:
                    return new Behaviors.ImageMirroring.Concrete.Rl4ImageMirrorer(sourceFile);
                case FileType.rl8:
                    return new Behaviors.ImageMirroring.Concrete.Rl8ImageMirrorer(sourceFile);
                default:
                    throw new NotImplementedException("Image mirrorer");
            }
        }
    }
}
