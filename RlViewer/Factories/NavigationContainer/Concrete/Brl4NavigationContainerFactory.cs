using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    class Brl4NavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            var brl4header = header as RlViewer.Headers.Concrete.Brl4.Brl4Header;
            if (brl4header == null) throw new ArgumentException("brl4header");
            return new Rl4NavigationContainer(properties.FilePath, brl4header.HeaderStruct.synthParams.D0, brl4header.HeaderStruct.synthParams.dD,
                brl4header.HeaderStruct.synthParams.board, brl4header.FileHeaderLength, brl4header.HeaderStruct.rlParams.width * brl4header.BytesPerSample);
        }
    }
}
