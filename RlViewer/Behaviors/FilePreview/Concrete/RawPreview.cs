using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilePreview.Concrete
{
    class RawPreview : Abstract.LocatorFilePreview
    {
        public RawPreview(string fileName, Headers.Abstract.LocatorFileHeader header)
            : base(header)
        {
            _fileName = fileName;
        }

        private string _fileName;


        public override HeaderInfoOutput GetPreview()
        {
            return new HeaderInfoOutput(_fileName, new List<Tuple<string, string>>());
        }
    }
}
