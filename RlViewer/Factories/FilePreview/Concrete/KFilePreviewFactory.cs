using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.FilePreview.Concrete
{
    class KFilePreviewFactory : Abstract.FilePreviewFactory
    {
        public override Behaviors.FilePreview.Abstract.LocatorFilePreview Create(string fileName, Headers.Abstract.LocatorFileHeader header)
        {
            return new Behaviors.FilePreview.Concrete.KPreview(fileName, header);
        }
    }
}
