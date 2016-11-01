using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageMirroring.Concrete
{
    class RImageMirrorer : Abstract.ImageMirrorer
    {
        public RImageMirrorer(Files.LocatorFile sourceFile)
            : base(sourceFile)
        {
            _rFile = (Files.Rli.Concrete.R)sourceFile;
            _rHeader = (Headers.Concrete.R.RHeader)_rFile.Header;
        }

        
        private Files.Rli.Concrete.R _rFile;
        protected override Files.LocatorFile SourceFile
        {
            get 
            {
                return _rFile;
            }
        }

        private Headers.Concrete.R.RHeader _rHeader;


        protected override byte[] InvertFlipType()
        {
            throw new NotImplementedException();
        }


    }
}
