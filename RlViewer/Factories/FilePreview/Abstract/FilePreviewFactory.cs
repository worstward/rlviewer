using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.FilePreview.Abstract
{

    public abstract class FilePreviewFactory
    {
        public abstract RlViewer.Behaviors.FilePreview.Abstract.LocatorFilePreview Create(string fileName, Headers.Abstract.LocatorFileHeader header);

        public static FilePreviewFactory GetFactory(RlViewer.Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Concrete.Brl4FilePreviewFactory();
                case FileType.rl4:
                    return new Concrete.Rl4FilePreviewFactory();
                case FileType.raw:
                    return new Concrete.RawFilePreviewFactory();
                case FileType.k:
                    return new Concrete.KFilePreviewFactory();
                case FileType.r:
                    return new Concrete.RFilePreviewFactory();
                case FileType.rl8:
                    return new Concrete.Rl8FilePreviewFactory();
                default:
                    throw new NotSupportedException("Unsupported file type");
            }
        }
    }

}
