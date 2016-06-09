using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    public class Rl4NavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            var rl4header = header as RlViewer.Headers.Concrete.Rl4.Rl4Header;
            if (rl4header == null) throw new ArgumentException("rl4header");
            return new Rl4NavigationContainer(properties.FilePath, rl4header.HeaderStruct.synthParams.D0, rl4header.HeaderStruct.rlParams.dx,
                rl4header.HeaderStruct.synthParams.board, rl4header.FileHeaderLength, rl4header.HeaderStruct.rlParams.width * rl4header.BytesPerSample,
                rl4header.HeaderStruct.rlParams.sx, rl4header.HeaderStruct.rlParams.sy);
        }
    }
}