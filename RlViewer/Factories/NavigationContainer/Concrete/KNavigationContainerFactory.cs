using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Navigation.Concrete;

namespace RlViewer.Factories.NavigationContainer.Concrete
{
    class KNavigationContainerFactory : Abstract.NavigationContainerFactory
    {
        public override Navigation.NavigationContainer Create(RlViewer.Files.FileProperties properties, Headers.Abstract.LocatorFileHeader header)
        {
            var kHeader = header as RlViewer.Headers.Concrete.K.KHeader;
            if (kHeader == null) throw new ArgumentException("kHeader");
            return new KNavigationContainer();
            //return new KNavigationContainer(properties.FilePath, kHeader.HeaderStruct.synthesisHeader.initialRange,
            //    kHeader.HeaderStruct.synthesisHeader.dx, kHeader.HeaderStruct.synthesisHeader.sideObservation,
            //    kHeader.FileHeaderLength, (int)kHeader.HeaderStruct.lineInfoHeader.lineLength * kHeader.BytesPerSample);
        }
    }
}
