using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Behaviors.ImageMirroring.Concrete
{
    class Brl4ImageMirrorer : Abstract.ImageMirrorer
    {
        public Brl4ImageMirrorer(Files.LocatorFile sourceFile)
            : base(sourceFile)
        {
            _brl4File = (Files.Rli.Concrete.Brl4)sourceFile;
            _brl4Header = (Headers.Concrete.Brl4.Brl4Header)_brl4File.Header;
        }

        
        private Files.Rli.Concrete.Brl4 _brl4File;
        protected override Files.LocatorFile SourceFile
        {
            get 
            {
                return _brl4File;
            }
        }

        private Headers.Concrete.Brl4.Brl4Header _brl4Header;


        protected override byte[] InvertFlipType()
        {
            var headerStruct = _brl4Header.HeaderStruct;
            var flipType = headerStruct.rlParams.flipType;
            var invertedFlipType = (byte)(1 - flipType);

            var brl4rliSubHeader = headerStruct.rlParams.ChangeFlipType(invertedFlipType);
            headerStruct.rlParams = brl4rliSubHeader;

            return Converters.StructIO.WriteStruct<Headers.Concrete.Brl4.Brl4RliFileHeader>(headerStruct);
        }


    }
}
