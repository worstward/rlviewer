using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    public class Rl8NavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            var rl8header = header as RlViewer.Headers.Concrete.Rl8.Rl8Header;
            if (rl8header == null) throw new ArgumentException("rl8header");
            return new Rl8NavigationContainer(properties.FilePath, rl8header.HeaderStruct.synthParams.D0, rl8header.HeaderStruct.rlParams.dx,
                rl8header.HeaderStruct.synthParams.board, rl8header.FileHeaderLength, rl8header.HeaderStruct.rlParams.width * rl8header.BytesPerSample,
                rl8header.HeaderStruct.rlParams.sx, rl8header.HeaderStruct.rlParams.sy);
        }
    }
}
