using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilePreview.Abstract
{
    public abstract class LocatorFilePreview
    {
        public LocatorFilePreview(Headers.Abstract.LocatorFileHeader header)
        {
            
        }

        public abstract HeaderInfoOutput GetPreview();
    }
}
