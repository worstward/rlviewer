using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageMirroring.Concrete
{
    class RawImageMirrorer : Abstract.ImageMirrorer
    {
        public RawImageMirrorer(Files.LocatorFile sourceFile)
            : base(sourceFile)
        {
            _rawFile = (Files.Rli.Concrete.Raw)sourceFile;
        }

        
        private Files.Rli.Concrete.Raw _rawFile;
        protected override Files.LocatorFile SourceFile
        {
            get 
            {
                return _rawFile;
            }
        }


        protected override byte[] InvertFlipType()
        {
            return new byte[0];
        }

    }
}
