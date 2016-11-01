using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Behaviors.ImageMirroring.Concrete
{
    class Rl4ImageMirrorer : Abstract.ImageMirrorer
    {
        public Rl4ImageMirrorer(Files.LocatorFile sourceFile)
            : base(sourceFile)
        {
            _rl4File = (Files.Rli.Concrete.Rl4)sourceFile;
            _rl4Header = (Headers.Concrete.Rl4.Rl4Header)_rl4File.Header;
        }

        
        private Files.Rli.Concrete.Rl4 _rl4File;
        protected override Files.LocatorFile SourceFile
        {
            get 
            {
                return _rl4File;
            }
        }

        private Headers.Concrete.Rl4.Rl4Header _rl4Header;


        protected override byte[] InvertFlipType()
        {
            var headerStruct = _rl4Header.HeaderStruct;
            var flipType = headerStruct.rlParams.flipType;
            var invertedFlipType = (byte)(1 - flipType);

            var rl4rliSubHeader = headerStruct.rlParams.ChangeFlipType(invertedFlipType);
            headerStruct.rlParams = rl4rliSubHeader;

            return Converters.StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4RliFileHeader>(headerStruct);
        }

    }
}
