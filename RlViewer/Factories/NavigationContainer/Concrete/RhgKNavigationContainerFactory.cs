using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    class RhgKNavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            var rHeader = header as RlViewer.Headers.Concrete.K.KHeader;
            if (rHeader == null) throw new ArgumentException("kHeader");

            //return new RhgKNavigationContainer(properties.FilePath, rHeader.HeaderStruct.synthesisHeader.initialRange,
            //    rHeader.HeaderStruct.synthesisHeader.dx, rHeader.HeaderStruct.synthesisHeader.sideObservation,
            //    rHeader.FileHeaderLength, (int)rHeader.HeaderStruct.lineInfoHeader.lineLength * rHeader.BytesPerSample);

            return new RhgKNavigationContainer(properties.FilePath);
        }
    }
}
